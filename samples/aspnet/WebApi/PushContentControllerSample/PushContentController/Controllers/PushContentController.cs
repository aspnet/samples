using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace PushContentController.Controllers
{
    /// <summary>
    /// This controller pushes data to the client as a never-ending HTTP response writing a little
    /// bit of data every 1 second. There can be N number of clients connected at any given time -- the
    /// list of clients is maintained in a concurrent dictionary. Clients are removed if writes fail
    /// which means that they have disconnected.
    /// </summary>
    /// <remarks>
    /// The sample needs to run using either full IIS/ASP or IIS Express. The Visual Studio Development
    /// Server does not work.
    /// </remarks>
    public class PushContentController : ApiController
    {
        private static readonly Lazy<Timer> _timer = new Lazy<Timer>(() => new Timer(TimerCallback, null, 0, 1000));
        private static readonly ConcurrentDictionary<StreamWriter, StreamWriter> _outputs = new ConcurrentDictionary<StreamWriter, StreamWriter>();

        public HttpResponseMessage GetUpdates(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new PushStreamContent((responseStream, httpContent, context) =>
            {
                StreamWriter responseStreamWriter = new StreamWriter(responseStream);

                // Register a callback which gets triggered when a client disconnects
                cancellationToken.Register(CancellationRequested, responseStreamWriter);

                _outputs.TryAdd(responseStreamWriter, responseStreamWriter);

            }, "text/plain");

            Timer t = _timer.Value;

            return response;
        }

        private void CancellationRequested(object state)
        {
            StreamWriter responseStreamWriter = state as StreamWriter;

            if (responseStreamWriter != null)
            {
                _outputs.TryRemove(responseStreamWriter, out responseStreamWriter);
            }
        }

        // Runs every second after the first request to this controller and
        // writes to the response streams of all currently active requests
        private static void TimerCallback(object state)
        {
            foreach (var kvp in _outputs.ToArray())
            {
                StreamWriter responseStreamWriter = kvp.Value;

                try
                {
                    responseStreamWriter.Write(DateTime.Now);
                    responseStreamWriter.Flush();
                }
                catch { }
            }
        }
    }
}