using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Hosting;
using Owin;

namespace CustomAssemblyResolver
{
    /// <summary>
    /// Sample illustrating loading ApiControllers from a dynamically loaded controller library
    /// assembly.
    /// </summary>
    class Program
    {
        static readonly string _baseAddress = "http://localhost:50231";

        static void Main(string[] args)
        {
            IDisposable server = null;

            try
            {
                server = WebApp.Start<Program>(url: _baseAddress);

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
                    server.Dispose();
                }
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

            // Set our own assembly resolver where we add the assemblies we need           
            CustomAssembliesResolver assemblyResolver = new CustomAssembliesResolver();
            config.Services.Replace(typeof(IAssembliesResolver), assemblyResolver);

            appBuilder.UseWebApi(config);
        }

        static async void RunClient()
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Send GET request to server for the hello controller which lives in the controller library
            Uri address = new Uri(_baseAddress + "/api/hello");
            HttpResponseMessage response = await client.GetAsync(address);

            // Ensure we get a successful response.
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Controller says {0}", content);
        }
    }
}