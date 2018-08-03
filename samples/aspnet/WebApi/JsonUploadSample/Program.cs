using System;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using JsonUploadSample.Models;

namespace JsonUploadSample
{
    /// <summary>
    /// This sample illustrates how to upload and download JSON to and from an ApiController.
    /// It runs a minimal ApiController and accesses it using HttpClient.
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                // Set up server configuration
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                // Create server
                server = new HttpSelfHostServer(config);

                // Start listening
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + _baseAddress);

                // Run HttpClient issuing requests
                RunClient();

                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
            }
        }

        /// <summary>
        /// Runs an HttpClient issuing a POST request against the controller.
        /// </summary>
        static async void RunClient()
        {
            HttpClient client = new HttpClient();

            Contact contact = new Contact
            {
                Name = "Henrik",
                Age = 100
            };

            // Post contact
            Uri address = new Uri(_baseAddress, "/api/contact");
            HttpResponseMessage response = await client.PostAsJsonAsync(address.ToString(), contact);

            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // Read result as Contact
            Contact result = await response.Content.ReadAsAsync<Contact>();

            Console.WriteLine("Result: Name: {0} Age: {1}", result.Name, result.Age);
        }
    }
}
