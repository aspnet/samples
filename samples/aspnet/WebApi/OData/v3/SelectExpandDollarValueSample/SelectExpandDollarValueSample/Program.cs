using SelectExpandDollarValueSample.Model;
using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;
using System.Net.Http;
using System.Threading.Tasks;

namespace SelectExpandDollarValueSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceUrl = "http://localhost:12345";
            using (WebApp.Start(serviceUrl, Configuration))
            {

                Console.WriteLine("Service listening at {0}", serviceUrl);
                Console.WriteLine("Press any key to stop the service and exit the application");
                QueryTheService(serviceUrl).Wait();
                Console.ReadKey();
            }
        }

        private static async Task QueryTheService(string serviceUrl)
        {
            await SendQuery(serviceUrl, "/odata/Customers", "Query the Customers entity set");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders", "Query the Customers entity set and expand the list of orders");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders/OrderItems", "Query the Customers entity set and expand the list of orders and the details of each order");
            await SendQuery(serviceUrl, "/odata/Customers?$select=Name", "Query the Customers entity set and select the name");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders&$select=Name,Orders/BillingAddress", "Query the Customers entity set and select the name of each customer and the biling address for their orders");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders/OrderItems&$select=Name,Orders/OrderItems/Name", "Query the Customers entity set and select the name of each customer and the name of each product on each order");
            await SendQuery(serviceUrl, "/odata/Customers(5)", "Query the Customer with Id = 5");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders", "Query the Customer with Id = 5 and expand the list of orders");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders/OrderItems", "Query the Customer with Id = 5 and expand the list of orders and the details of each order");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$select=Name", "Query the Customer with Id = 5 and select the name");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders&$select=Name,Orders/BillingAddress", "Query the Customer with Id = 5 and select the name of the customer and the biling address for their orders");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders/OrderItems&$select=Name,Orders/OrderItems/Name", "Query the Customer with Id = 5 and select the name of the customer and the name of each product on each order");
            await SendQuery(serviceUrl, "/odata/Customers(5)/Name/$value", "Query the raw value of the Name property from the Customer with Id = 5");

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
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.Routes.MapODataRoute("odata", "odata", ShoppingEdmModel.GetModel());
            builder.UseWebApi(configuration);
        }
    }
}
