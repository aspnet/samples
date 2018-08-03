using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataQueryLimitationsSample.Model;
using Owin;

namespace ODataQueryLimitationsSample
{
    class Program
    {
        private static readonly string serverUrl = "http://localhost:12345";
        static void Main(string[] args)
        {
            using (WebApp.Start(serverUrl, Configuration))
            {
                Console.WriteLine("Server listening at {0}", serverUrl);
                RunQueries();
                Console.WriteLine("Press any key to exit...");
                Console.Read();
            }
        }

        private static void RunQueries()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;

            // Trying to order by an unsortable property will result in an exception.
            // The property has been marked as unsortable using the Unsortable attribute.
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers?$orderby=Name");
            response = client.SendAsync(request).Result;
            Console.WriteLine("Sending request to sort by an unsortable property");
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Trying to filter by a non filterable property will result in an exception.
            // The property has been marked as non filterable using the NonFilterable attribute.
            Console.WriteLine("Sending request to filter by an unfilterable property");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers?$filter=Name le 'Name 5'");
            response = client.SendAsync(request).Result;
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Trying to expand a not expandable property will result in an exception.
            // The property has been marked as not expandable using the NotExpandable attribute.
            Console.WriteLine("Sending request to expand an non expandable property");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers?$expand=Orders");
            response = client.SendAsync(request).Result;
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // The path used in the filter contains a non filterable property (Address) whose limitation has been overriden
            // using the model builder and another property (FirstName) that is also non filterable and that causes the query to fail.
            // All the properties in the $filter path (Address and FirstLine) must be filterable.
            Console.WriteLine("Sending a request with a filter operation that contains a non filterable property in the path of the filter expression.");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers?$filter=Address/FirstLine eq 'First Line 5'");
            response = client.SendAsync(request).Result;
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // The path used in the filter contains only filterable properties, so the query succeeds.
            Console.WriteLine("Sending a request with a filter operation that doesn't contain a non filterable property in the path of the filter expression.");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers?$filter=Address/ZipCode eq 365");
            response = client.SendAsync(request).Result;
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
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

            EntitySetConfiguration<Customer> customers = builder.EntitySet<Customer>("Customers");
            customers.EntityType.ComplexProperty(c => c.Address).IsFilterable().IsSortable();
            builder.EntitySet<Order>("Orders");

            return builder.GetEdmModel();
        }
    }
}
