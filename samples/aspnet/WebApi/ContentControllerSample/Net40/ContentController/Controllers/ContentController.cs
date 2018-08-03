using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncRequestReaderSample.Controllers
{
    /// <summary>
    /// This sample controller illustrates how to deal with request entities and response entities as streams.
    /// It has two actions: a PUT action which reads the request entity body asynchronously and stores it in
    /// a local file and a GET action which returns the content of the local file.
    /// </summary>
    /// <remarks>
    /// For illustration reasons the controller simply returns 503 Service unavailable if a client tries to PUT and GET 
    /// the contents at the same time.
    /// </remarks>
    public class ContentController : ApiController
    {
        const int BufferSize = 32 * 1024;
        static readonly string _store = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("D"));

        public Task<HttpResponseMessage> GetContent()
        {
            if (!File.Exists(_store))
            {
                return Task.Factory.StartNew(() => Request.CreateResponse(HttpStatusCode.NotFound));
            }

            // Open file and read response from it. If read fails then return 503 Service Not Available           
            try
            {
                // Create StreamContent from FileStream. FileStream will get closed when StreamContent is closed
                FileStream fStream = new FileStream(_store, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true);
                HttpResponseMessage response = Request.CreateResponse();
                response.Content = new StreamContent(fStream);
                return Task.Factory.StartNew(() => response);
            }
            catch (Exception e)
            {
                return Task.Factory.StartNew(() => Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, e));
            }
        }

        public Task<HttpResponseMessage> PutContent()
        {
            // Determine whether this is first time accessing the file so that we can return 201 Created.
            bool first = !File.Exists(_store);

            // Open file and write request to it. If write fails then return 503 Service Not Available
            FileStream fStream = null;
            try
            {
                fStream = new FileStream(_store, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, useAsync: true);

                // Copy content asynchronously to local store
                return Request.Content.CopyToAsync(fStream).ContinueWith(
                    (copyTask) =>
                    {
                        if (copyTask.IsCanceled || copyTask.IsFaulted)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, copyTask.Exception);
                        }

                        fStream.Close();

                        // Return either 200 Ok or 201 Created response
                        return Request.CreateResponse(first ? HttpStatusCode.Created : HttpStatusCode.OK);
                    });
            }
            catch (Exception e)
            {
                if (fStream != null)
                {
                    fStream.Close();
                }
                return Task.Factory.StartNew(() => Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, e));
            }
        }
    }
}