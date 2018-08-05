using System;
using System.IO;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ResponseEntityProcessorSample.Handlers;

namespace ResponseEntityProcessorSample
{
    /// <summary>
    /// This sample illustrates how to copy a response entity (body) to a local file and perform additional processing on that 
    /// file asynchronously. It does so by hooking in a HttpMessageHandler that wraps the response entity with one that both
    /// writes itself to the output as normal and to a local file.
    /// </summary>
    class Program
    {
        const string TempFolder = @"output";

        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                // Set up server configuration
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;

                // Add a route
                config.Routes.MapHttpRoute(
                  name: "default",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { controller = "Home", id = RouteParameter.Optional });

                // Set up ResponseEntityHandler with a temporary folder
                Directory.CreateDirectory(TempFolder);
                DelegatingHandler responseEntityHandler = new ResponseEntityHandler(TempFolder);

                // Add our sample message handler to the server configuration
                config.MessageHandlers.Add(responseEntityHandler);

                server = new HttpSelfHostServer(config);
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

        static async void RunClient()
        {
            HttpClient client = new HttpClient();

            // Issue GET to multiple times to see the ResponseEntityHandler in action
            for (int count = 0; count < 5; count++)
            {
                HttpResponseMessage response = await client.GetAsync(_baseAddress + "api/values");

                // Check that response was successful or throw exception
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
