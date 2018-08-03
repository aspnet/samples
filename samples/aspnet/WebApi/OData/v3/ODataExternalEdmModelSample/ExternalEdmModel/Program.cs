using System;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ExternalEdmModel.Model;
using Microsoft.Data.Edm;
using Microsoft.Owin.Hosting;
using Owin;

namespace ExternalEdmModel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Database.SetInitializer<CustomersContext>(new CustomersContextInitializer());

            string serviceUrl = "http://localhost:12345";
            using (WebApp.Start(serviceUrl, Configuration))
            {
                Console.WriteLine("Server listening at {0}", serviceUrl);

                QueryTheService(serviceUrl).Wait();

                Console.WriteLine("Press Any Key to Exit ...");
                Console.ReadKey();
            }
        }

        private static async Task QueryTheService(string serviceUrl)
        {
            await SendQuery(serviceUrl, "/odata/Customers", "Query the Customers entity set");
            await SendQuery(serviceUrl, "/odata/Orders", "Query the Orders entity set");
        }

        private static async Task SendQuery(string serviceUrl, string query, string queryDescription)
        {
            Console.WriteLine("Executing {0}...", queryDescription);

            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + query);

            HttpResponseMessage response = await client.SendAsync(request);

            Console.WriteLine("\r\nResult:");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static void Configuration(IAppBuilder builder)
        {
            IEdmModel model;
            using (CustomersContext context = new CustomersContext())
            {
                model = context.GetEdmModel();
            }

            HttpConfiguration configuration = new HttpConfiguration();
            configuration.Routes.MapODataRoute("odata", "odata", model);

            builder.UseWebApi(configuration);
        }
    }
}
