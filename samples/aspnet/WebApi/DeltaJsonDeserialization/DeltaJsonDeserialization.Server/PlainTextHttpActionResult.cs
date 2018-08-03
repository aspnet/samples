using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DeltaJsonDeserialization.Server
{
    public class PlainTextHttpActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _text;

        public PlainTextHttpActionResult(HttpRequestMessage request, string text)
        {
            _request = request;
            _text = text;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response = _request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(_text);
            return response;
        }
    }
}