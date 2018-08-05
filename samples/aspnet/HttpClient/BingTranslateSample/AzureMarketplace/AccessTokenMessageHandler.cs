using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BingTranslate.AzureMarketplace
{
    /// <summary>
    /// Simple DelegatingHandler which obtains an Access Token from Azure Marketplace
    /// by issuing an asynchronous HTTP POST request and inserting the response into the 
    /// original HTTP request. 
    /// </summary>
    public class AccessTokenMessageHandler : DelegatingHandler
    {
        // You need to obtain a valid application key from the Azure Marketplace,
        // see http://msdn.microsoft.com/en-us/library/hh454950.aspx for details
        private readonly string _clientId = "Your client application name";
        private readonly string _clientSecret = "Youc client application secret";

        private readonly Uri _dataMarketAddress = new Uri("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13");

        // We use a single HttpClient instance for all requests to Azure Marketplace
        private HttpClient _client = new HttpClient();

        public AccessTokenMessageHandler(HttpMessageHandler innerContent)
            : base(innerContent)
        {
        }

        /// <summary>
        /// This message handler first sends an async form POST request to the Azure Marketplace to obtain an Access Token.
        /// Once we obtain an access token, we add that as an authentication header to the original request and submit
        /// that request asynchronously to the BING translation service.
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // First we issue async HTML form POST request to the Azure Marketplace to obtain an Access Token. 
            // See http://msdn.microsoft.com/en-us/library/hh454950.aspx for details

            // Create form parameters that we will send to data market.
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id",  _clientId},
                { "client_secret", _clientSecret },
                { "scope", "http://api.microsofttranslator.com" }
            };

            FormUrlEncodedContent authentication = new FormUrlEncodedContent(properties);
            HttpResponseMessage dataMarketResponse = await _client.PostAsync(_dataMarketAddress, authentication);

            // If client authentication failed then we get a JSON response from Azure Market Place
            if (!dataMarketResponse.IsSuccessStatusCode)
            {
                JToken error = await dataMarketResponse.Content.ReadAsAsync<JToken>();
                string errorType = error.Value<string>("error");
                string errorDescription = error.Value<string>("error_description");
                throw new HttpRequestException(string.Format("Azure market place request failed: {0} {1}", errorType, errorDescription));
            }

            // Get the access token to attach to the original request from the response body
            AdmAccessToken accessToken = await dataMarketResponse.Content.ReadAsAsync<AdmAccessToken>();

            // Add Authorization header with our access token that we got from data market
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            // Submit the original request updated the access token
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
