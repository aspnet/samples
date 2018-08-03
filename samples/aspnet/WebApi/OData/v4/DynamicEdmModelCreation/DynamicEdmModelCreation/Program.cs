using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace DynamicEdmModelCreation
{
    public class Program
    {
        private static readonly string serviceUrl = "http://localhost:12345";

        public static void Main(string[] args)
        {
            using (WebApp.Start(serviceUrl, Configuration))
            {
                Console.WriteLine("Server listening at {0}", serviceUrl);

                QueryTheService().Wait();

                Console.WriteLine("Press Any Key to Exit ...");
                Console.ReadKey();
            }
        }

        private static async Task QueryTheService()
        {
            await SendQuery("/odata/mydatasource/", "Query service document.");
            await SendQuery("/odata/mydatasource/$metadata", "Query $metadata.");
            await SendQuery("/odata/mydatasource/Products", "Query the Products entity set.");
            await SendQuery("/odata/mydatasource/Products(1)", "Query a Product entry.");

            await SendQuery("/odata/anotherdatasource/", "Query service document.");
            await SendQuery("/odata/anotherdatasource/$metadata", "Query $metadata.");
            await SendQuery("/odata/anotherdatasource/Students", "Query the Students entity set.");
            await SendQuery("/odata/anotherdatasource/Students(100)", "Query a Student entry.");
        }

        private static async Task SendQuery(string query, string queryDescription)
        {
            Console.WriteLine("Sending request to: {0}. Executing {1}...", query, queryDescription);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + query);
            HttpResponseMessage response = await client.SendAsync(request);

            Console.WriteLine("\r\nResult:");
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.WriteLine();
        }

        private static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            builder.UseWebApi(configuration);
        }
    }
}