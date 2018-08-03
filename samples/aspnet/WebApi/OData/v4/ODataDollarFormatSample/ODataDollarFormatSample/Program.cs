using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataDollarFormatSample.Model;
using Owin;

namespace ODataDollarFormatSample
{
    class Program
    {
        private static readonly string serviceUrl = "http://localhost:12345";
        static void Main(string[] args)
        {
            using (WebApp.Start(serviceUrl, Configuration))
            {
                Console.WriteLine("Server listening at {0}", serviceUrl);
                RunQueries();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void RunQueries()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;

            // Sending a request for customers with $format using a MIME type.
            // The response will be JSON with minimal metadata (which is also the default format, that is, it is the same as without $format).
            request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/odata/Customers?$format=application%2Fjson");
            Console.WriteLine("Sending a request for customers with $format using a MIME type");
            Console.WriteLine("Request URI:");
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Sending a request for customers with $format using a MIME type with parameters.
            // The response will be in JSON full metadata instead of JSON minimal metadata (which is the default format).
            request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/odata/Customers?$format=application%2Fjson%3Bodata.metadata%3Dfull");
            Console.WriteLine("Sending a request for customers with $format using a MIME type with parameters");
            Console.WriteLine("Request URI:");
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Sending the same request as the first one but using an alias for the format instead of the MIME type.
            // We use json instead of application%2Fjson.
            request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/odata/Customers?$format=json");
            Console.WriteLine("Sending a request for customers with $format using an alias for the format");
            Console.WriteLine("Request URI:");
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            // Sending a request for customers with $format and Accept header.
            // The service will use the value from the $format query option instead of the value of the Accept header.
            // The response will be in JSON with minimal metadata.
            request = new HttpRequestMessage(HttpMethod.Get, serviceUrl + "/odata/Customers?$format=application%2Fjson");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json;odata.metadata=full"));
            Console.WriteLine("Sending a request for customers with $format to override the Accept header");
            Console.WriteLine("Request URI:");
            Console.WriteLine();
            Console.WriteLine(request.RequestUri);
            Console.WriteLine();
            Console.WriteLine("Request Accept header:");
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", request.Headers.Accept.Select(c => c.ToString())));
            Console.WriteLine();
            Console.WriteLine("Response:");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
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
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }
}
