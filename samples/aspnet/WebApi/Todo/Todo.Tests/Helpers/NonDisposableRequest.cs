using System.Net.Http;

namespace Todo.Tests.Helpers
{
    public class NonDisposableRequest : HttpRequestMessage
    {
        public NonDisposableRequest(HttpRequestMessage request)
        {
            CopyRequest(request, this);
        }

        public HttpRequestMessage GetDisposableRequest()
        {
            return CopyRequest(this);
        }

        HttpRequestMessage CopyRequest(HttpRequestMessage request, HttpRequestMessage newRequest = null)
        {
            newRequest = newRequest ?? new HttpRequestMessage();
            newRequest.Content = request.Content;
            newRequest.Method = request.Method;
            newRequest.RequestUri = request.RequestUri;
            newRequest.Version = request.Version;
            foreach (var header in request.Headers)
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            foreach (var property in request.Properties)
            {
                newRequest.Properties.Add(property);
            }
            return newRequest;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
