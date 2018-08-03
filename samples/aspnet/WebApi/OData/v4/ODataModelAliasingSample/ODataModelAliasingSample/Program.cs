using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataModelAliasingSample.Model;
using Owin;

namespace ODataModelAliasingSample
{
    class Program
    {
        private static readonly string serverUrl = "http://localhost:12345";
        static void Main(string[] args)
        {
            using (WebApp.Start(serverUrl, Configuration))
            {
                Console.WriteLine("Server listening at: {0}", serverUrl);
                RunQueries();
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
        }

        private static void RunQueries()
        {
            HttpClient client = new HttpClient();

            // Showing the aliased names in the $metadata document.
            // The metadata will display Customer instead of CustomerDto, etc.
            Console.WriteLine("Showing the metadata document with the aliased names");
            Console.WriteLine();
            Console.WriteLine(client.GetStringAsync(serverUrl + "/odata/$metadata").Result);
            Console.WriteLine();

            // Querying for a customer to see the aliased payload.
            // Check that the payload reflects that GivenName is aliased to FirstName.
            Console.WriteLine("Querying a single customer at /Customers(1):");
            Console.WriteLine();
            Console.WriteLine(client.GetStringAsync(serverUrl + "/odata/Customers").Result);
            Console.WriteLine();

            // Querying the customers feed and using the aliased property name on a $filter clause.
            // Look at the FirstName property in the query string instead of GivenName as in the CLR object.
            Console.WriteLine("Querying for the feed of customers and filtering on an aliased property:");
            Console.WriteLine();
            Console.WriteLine(client.GetStringAsync(serverUrl + "/odata/Customers?$filter=FirstName le 'First name 5'").Result);
            Console.WriteLine();

            // Querying the orders of a customer.
            // Look at the Orders property in the path of the URI instead of Purchases on the CLR type name.
            Console.WriteLine("Querying for the orders associated to a customer:");
            Console.WriteLine();
            Console.WriteLine(client.GetStringAsync(serverUrl + "/odata/Customers(1)/Orders").Result);
        }

        private static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", GetModel());

            builder.UseWebApi(configuration);
        }

        private static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<CustomerDto> customers = builder.EntitySet<CustomerDto>("Customers");
            EntitySetConfiguration<OrderDto> orders = builder.EntitySet<OrderDto>("Orders");
            orders.EntityType.Name = "Order";
            orders.EntityType.Property(p => p.Total).Name = "Check";
            return builder.GetEdmModel();
        }
    }
}
