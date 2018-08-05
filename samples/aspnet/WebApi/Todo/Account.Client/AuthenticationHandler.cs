using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Client
{
    public class AuthenticationHandler : DelegatingHandler
    {
        public AuthenticationHandler()
        {
            MaxRetries = 2;
        }

        public int MaxRetries { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            int retries = 0;
            while (response != null && response.StatusCode == HttpStatusCode.Unauthorized && retries < MaxRetries)
            {
                if (!await HandleUnauthorizedAsync(request, response, cancellationToken))
                {
                    break;
                }
                response.Dispose();
                response = await base.SendAsync(request, cancellationToken);
                retries++;
            }
            // If the response is still Unauthorized  after MaxRetries, just return that response.
            return response;
        }

        protected virtual Task<bool> HandleUnauthorizedAsync(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return Task.FromResult(false);
        }

    }
}
