using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ActionResults.Results
{
    public class OkTextPlainResult : IHttpActionResult
    {
        private readonly ApiController _controller;

        public OkTextPlainResult(string content, Encoding encoding, ApiController controller)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            Content = content;
            Encoding = encoding;
            _controller = controller;
        }

        public string Content { get; private set; }

        public Encoding Encoding { get; private set; }

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
            response.RequestMessage = Request;
            response.Content = new StringContent(Content, Encoding);
            return response;
        }
    }
}