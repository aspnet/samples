using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RelaySample.Controllers
{
    /// <summary>
    /// This sample shows how to relay a response from a backend service asynchronously 
    /// without buffering the content on the server.
    /// </summary>
    public class RelayController : ApiController
    {
        // Create a shared instance of HttpClient and set the request timeout
        private static readonly HttpClient _client = new HttpClient()
        {
            Timeout = TimeSpan.FromMinutes(20)
        };

        // Our sample service which we relay
        private static readonly Uri _service = new Uri("http://localhost:50231/api/content");

        public async Task<HttpResponseMessage> GetRelay()
        {
            // Submit request to our service which we relay. We only ask for the headers first so that we don't read the entire response just yet.
            using (HttpResponseMessage serviceResponse = await _client.GetAsync(_service, HttpCompletionOption.ResponseHeadersRead))
            {
                // Return response
                return CreateResponse(serviceResponse);
            }
        }

        public async Task<HttpResponseMessage> PutRelay()
        {
            // Get content (which hasn't been read yet) from incoming request.
            HttpContent content = Request.Content;
            Request.Content = null;

            // Submit request to our service which we relay. 
            using (HttpResponseMessage serviceResponse = await _client.PutAsync(_service, content))
            {
                // Return response
                return CreateResponse(serviceResponse);
            }
        }

        private HttpResponseMessage CreateResponse(HttpResponseMessage serviceResponse)
        {
            // Create a new HttpResponseMessage so that we don't carry over server specific headers from the
            // service. Initialize it with the status code from the service response
            HttpResponseMessage relayResponse = Request.CreateResponse(serviceResponse.StatusCode);

            // Simply set the relay response content to that of the service response
            // We also clear the content from the service response so that it doesn't
            // get disposed when the service response is disposed.
            relayResponse.Content = serviceResponse.Content;
            serviceResponse.Content = null;

            // We now have the completed relay response which we can return
            return relayResponse;
        }
    }
}
