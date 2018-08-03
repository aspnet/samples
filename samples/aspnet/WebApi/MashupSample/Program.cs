using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using MashupSample.Models;

namespace MashupSample
{
    /// <summary>
    /// This sample shows how to asynchronously access multiple remote sites from
    /// within an ApiController action. Each time the action is hit, the requests 
    /// are performed.
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;

            try
            {
                // Create configuration
                var config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;

                // Add a route
                config.Routes.MapHttpRoute(
                  name: "default",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { controller = "Home", id = RouteParameter.Optional });

                // Create server
                server = new HttpSelfHostServer(config);

                // Start server
                server.OpenAsync().Wait();

                RunClient();

                Console.WriteLine("Server listening at {0}", _baseAddress);
                Console.WriteLine("Hit ENTER to exit");
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
                    // Close server
                    server.CloseAsync().Wait();
                }
            }
        }

        static async void RunClient()
        {
            HttpClient client = new HttpClient();

            // Get the results for a query for topic 'microsoft'
            Uri address = new Uri(_baseAddress, "/api/mashup?topic=microsoft");
            HttpResponseMessage response = await client.GetAsync(address);

            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // Read response asynchronously as string and write out
            List<Story> content = await response.Content.ReadAsAsync<List<Story>>();

            Console.WriteLine("Got result querying for 'microsoft' against mashup service");
            foreach (Story story in content)
            {
                Console.WriteLine("Story from {0}: {1}", story.Source, story.Description);
            }
        }
    }
}
