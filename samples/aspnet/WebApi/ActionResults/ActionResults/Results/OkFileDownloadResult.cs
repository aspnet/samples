using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ActionResults.Results
{
    public class OkFileDownloadResult : IHttpActionResult
    {
        private readonly ApiController _controller;

        public OkFileDownloadResult(string localPath, string contentType, string downloadFileName,
            ApiController controller)
        {
            if (localPath == null)
            {
                throw new ArgumentNullException("localPath");
            }

            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            if (downloadFileName == null)
            {
                throw new ArgumentNullException("downloadFileName");
            }

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            LocalPath = localPath;
            ContentType = contentType;
            DownloadFileName = downloadFileName;
            _controller = controller;
        }

        public string LocalPath { get; private set; }

        public string ContentType { get; private set; }

        public string DownloadFileName { get; private set; }

        public HttpRequestMessage Request
        {
            get { return _controller.Request; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(File.OpenRead(MapPath(LocalPath)));
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = DownloadFileName
            };
            return response;
        }

        private static string MapPath(string path)
        {
            // The following code is for demonstration purposes only and is not fully robust for production usage.
            // HttpContext.Current is not always available after asynchronous calls complete.
            // Also, this call is host-specific and will need to be modified for other hosts such as OWIN.
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}