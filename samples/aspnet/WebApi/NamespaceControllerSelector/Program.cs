using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin.Hosting;
using Owin;

namespace NamespaceControllerSelectorSample
{
    class Program
    {
        static readonly string _baseAddress = "http://localhost:50231/";

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

            // Register default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{namespace}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Replace the default controller selector with a custom one
            // which considers namespace information to select controllers.
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            appBuilder.UseWebApi(config);
        }

        static void RunClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);

            using (HttpResponseMessage response = client.GetAsync("api/v1/values").Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Version 1 response: '{0}'\n", content);
            }

            using (HttpResponseMessage response = client.GetAsync("api/v2/values").Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Version 2 response: '{0}'\n", content);
            }
        }
    }
}
