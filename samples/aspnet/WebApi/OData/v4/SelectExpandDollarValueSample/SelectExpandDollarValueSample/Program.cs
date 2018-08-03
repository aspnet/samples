using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData;
using System.Web.OData.Extensions;
using Microsoft.Owin.Hosting;
using Owin;
using SelectExpandDollarValueSample.Model;

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
                QueryTheService(serviceUrl).Wait();
                Console.WriteLine("Press any key to stop the service and exit the application");
                Console.ReadKey();
            }
        }

        private static async Task QueryTheService(string serviceUrl)
        {
            await SendQuery(serviceUrl, "/odata/Customers", "Query the Customers entity set");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders", "Query the Customers entity set and expand the list of orders");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders($expand=OrderItems)", "Query the Customers entity set and expand the list of orders and the details of each order");
            await SendQuery(serviceUrl, "/odata/Customers?$select=Name", "Query the Customers entity set and select the name");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders($select=BillingAddress)&$select=Name", "Query the Customers entity set and select the name of each customer and the biling address for their orders");
            await SendQuery(serviceUrl, "/odata/Customers?$expand=Orders($expand=OrderItems($select=Name);$select=OrderItems)&$select=Name", "Query the Customers entity set and select the name of each customer and the name of each product on each order");
            await SendQuery(serviceUrl, "/odata/Customers(5)", "Query the Customer with Id = 5");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders", "Query the Customer with Id = 5 and expand the list of orders");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders($expand=OrderItems)", "Query the Customer with Id = 5 and expand the list of orders and the details of each order");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$select=Name", "Query the Customer with Id = 5 and select the name");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders($select=BillingAddress)&$select=Name", "Query the Customer with Id = 5 and select the name of the customer and the biling address for their orders");
            await SendQuery(serviceUrl, "/odata/Customers(5)?$expand=Orders($expand=OrderItems($select=Name);$select=OrderItems)&$select=Name", "Query the Customer with Id = 5 and select the name of the customer and the name of each product on each order");
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

            // ODataNullValueMessageHandler handle returning a 404 response instead of a 200 with a null value when the raw value
            // of a property is null.
            var nullHandler = new ODataNullValueMessageHandler
            {
                InnerHandler = new HttpControllerDispatcher(configuration)
            };

            configuration.MapODataServiceRoute("odata", "odata", ShoppingEdmModel.GetModel(), nullHandler);
            builder.UseWebApi(configuration);
        }
    }
}
