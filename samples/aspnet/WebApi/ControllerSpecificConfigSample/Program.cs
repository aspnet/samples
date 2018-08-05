using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace ControllerSpecificConfigSample
{
    /// <summary>
    /// This sample shows how to customize the parameter binding process by registering 
    /// custom ParameterBinding for certain types. 
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            // Set up configuration
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
            config.HostNameComparisonMode = HostNameComparisonMode.Exact;

            // Set up default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            HttpSelfHostServer server = null;
            try
            {
                // Create server 
                server = new HttpSelfHostServer(config);

                // Start server
                server.OpenAsync().Wait();
                Console.WriteLine("Server listening at {0}", _baseAddress);

                RunClient();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
            }
            finally
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
            }
        }

        private static async void RunClient()
        {
            // start the client
            HttpClient client = new HttpClient
            {
                BaseAddress = _baseAddress
            };

            // Verify that we bind multiple parameters from request for POST
            StringContent content = new StringContent("FirstName=Hongmei&LastName=Ge", Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage postResponse = await client.PostAsync("/api/sample", content);
            postResponse.EnsureSuccessStatusCode();
            string result = await postResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Received response to POST: {0}", result);

            // Verify that we have the plain text formatter by asking for plain text
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(_baseAddress, "/api/sample"),
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            HttpResponseMessage getResponse = await client.SendAsync(request);
            getResponse.EnsureSuccessStatusCode();
            Console.WriteLine("Received response to GET: {0}", getResponse);
        }
    }
}
