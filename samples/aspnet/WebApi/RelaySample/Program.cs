using System;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace RelaySample
{
    /// <summary>
    /// This sample shows how to relay a response from a backend service without
    /// buffering the content on the server. Please see the README for details.
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

                // Turn on streaming in selfhost for both requests and responses.
                // if you do this in ASP.NET then please see this blog from @filip_woj for details 
                // on how to turn off buffering in ASP.NET: 
                // http://www.strathweb.com/2012/09/dealing-with-large-files-in-asp-net-web-api/
                config.TransferMode = TransferMode.Streamed;

                // Set max received size to 10G
                config.MaxReceivedMessageSize = 10L * 1024 * 1024 * 1024;

                // Set receive and send timeout to 20 mins
                config.ReceiveTimeout = TimeSpan.FromMinutes(20);
                config.SendTimeout = TimeSpan.FromMinutes(20);

                // Add a route
                config.Routes.MapHttpRoute(
                  name: "default",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { controller = "Home", id = RouteParameter.Optional });

                // Create server
                server = new HttpSelfHostServer(config);

                // Start server
                server.OpenAsync().Wait();

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
    }
}
