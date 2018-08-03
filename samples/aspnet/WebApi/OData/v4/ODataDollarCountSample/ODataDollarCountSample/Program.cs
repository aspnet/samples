using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataDollarCountSample.Model;
using Owin;

namespace ODataDollarCountSample
{
    class Program
    {
        private static readonly string _baseAddress = string.Format("http://{0}:12345", Environment.MachineName);
        private static readonly HttpClient _httpClient = new HttpClient();

        static void Main(string[] args)
        {
            using (WebApp.Start(_baseAddress, Configuration))
            {
                Console.WriteLine("Server listening at {0}", _baseAddress);

                _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                string requestUri = "";
                HttpResponseMessage response = null;

                // Request that returns the Customers count.
                requestUri = _baseAddress + "/odata/Customers/$count";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Request that returns the Emails count.
                requestUri = _baseAddress + "/odata/Customers(1)/Emails/$count";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Request that returns the ShipAddresses count in which the City equals 'Shanghai'.
                requestUri = _baseAddress + "/odata/Customers(1)/ShipAddresses/$count?$filter=City eq 'Shanghai'";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Requests that hits "The property 'AvailableDays' cannot be used for $count." error,
                // because the property AvailableDays is decorated with [NotCountable].
                requestUri = _baseAddress + "/odata/Customers(1)/AvailableDays/$count";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
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
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        private static HttpResponseMessage Get(string requestUri)
        {
            HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;
            return response;
        }

        private static void Comment(string message)
        {
            Console.WriteLine(message);
        }

        private static void Comment(HttpResponseMessage response)
        {
            Console.WriteLine(response);

            string content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
    }
}
