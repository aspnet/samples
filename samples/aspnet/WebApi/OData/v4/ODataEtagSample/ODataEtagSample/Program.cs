using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using ODataEtagSample.Model;
using Owin;

namespace ODataEtagSample
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
                Console.ReadKey();
            }
        }

        private static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            IEdmModel model = GetModel();
            configuration.
                MapODataServiceRoute(
                    routeName: "odata",
                    routePrefix: "odata",
                    model: model,
                    pathHandler: new DefaultODataPathHandler(),
                    routingConventions: ODataRoutingConventions.CreateDefaultWithAttributeRouting(configuration, model),
                    defaultHandler: HttpClientFactory.CreatePipeline(
                        innerHandler: new HttpControllerDispatcher(configuration),
                        handlers: new[] { new ETagMessageHandler() }));
            builder.UseWebApi(configuration);
        }

        private static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Customer> customers = builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        private static void RunQueries()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;

            // Retrieving an entity for the first time. Observe that the ETag is in the response headers and 
            // the returned payload contains the annotation @odata.etag indicating the ETag associated with that customer.
            Console.WriteLine("Retrieving a single customer at {0}/odata/Customers(1)", serverUrl);
            Console.WriteLine();
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers(1)");
            response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.ToString());
            dynamic customer = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine(customer);
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");
            string etag = customer["@odata.etag"];

            // Retrieving the same customer as in the previous request but only if the ETag doesn't match the one
            // specified in the If-None-Match header. We are sending the ETag value that we obtained from the previous
            // request, so we expect to see a 304 (Not Modified) response.
            Console.WriteLine("Retrieving the customer at {0}/odata/Customers(1) when the Etag value sent matches");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers(1)");
            request.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Parse(etag));
            response = client.SendAsync(request).Result;
            Console.WriteLine("The response status code is: {0}", response.StatusCode);
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");

            // Retrieving the same customer as in the previous request but only if the ETag doesn't match the one
            // specified in the If-None-Match header. We are sending a different ETag value, so we expect to see a 200
            // (OK) response.
            Console.WriteLine("Retrieving the customer at {0}/odata/Customers(1) when the Etag value sent matches");
            request = new HttpRequestMessage(HttpMethod.Get, serverUrl + "/odata/Customers(1)");
            request.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Parse("W/\"MQ==\""));
            response = client.SendAsync(request).Result;
            Console.WriteLine("The response status code is {0}", response.StatusCode);
            Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");

            // Removing the annotations from the customer object as they are not required on the following requests and
            // changing the age value.
            customer.Age = 99;
            customer.Remove("@odata.etag");
            customer.Remove("@odata.context");

            // Trying to update the customer using a different ETag value than the ETag on the previous request. The
            // server will return a 412 (Precondition Failed) response.
            request = new HttpRequestMessage(HttpMethod.Put, serverUrl + "/odata/Customers(1)");
            // Setting up a different ETag value.
            request.Headers.IfMatch.Add(EntityTagHeaderValue.Parse("W/\"MQ==\""));
            request.Content = new ObjectContent<JObject>(customer, new JsonMediaTypeFormatter());
            Console.WriteLine("Trying to update the Customer using a different ETag value on the If-Match header and failing");
            response = client.SendAsync(request).Result;
            Console.WriteLine("The response status code is {0}", response.StatusCode);
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");

            // Trying to update the customer using the ETag value retrieved from the first request. The server will
            // process the request and return a 200 (OK) response.
            request = new HttpRequestMessage(HttpMethod.Put, serverUrl + "/odata/Customers(1)");
            request.Headers.IfMatch.Add(EntityTagHeaderValue.Parse(etag));
            request.Content = new ObjectContent<JObject>(customer, new JsonMediaTypeFormatter());
            Console.WriteLine("Trying to update a Customer using the same ETag value on the If-Match header and succeeding");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.Write(JObject.Parse(response.Content.ReadAsStringAsync().Result));
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");

            // Trying to update the customer using the If-Match header and sending an entity that doesn't exist on the
            // database. The behavior in this case is "strict update", meaning that the server shouldn't try to insert
            // the entity instead, so the answer we receive is 404 (Not Found).
            request = new HttpRequestMessage(HttpMethod.Put, serverUrl + "/odata/Customers(30)");
            request.Headers.IfMatch.Add(EntityTagHeaderValue.Parse("W/\"MQ==\""));
            Customer newCustomer = new Customer
            {
                Id = 30,
                Name = "New customer",
                Age = 30,
                Version = 0
            };
            request.Content = new ObjectContent<Customer>(newCustomer, new JsonMediaTypeFormatter());
            Console.WriteLine("Trying to update a non existing customer with the If-Match header present");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.WriteLine("The response status code is {0}", response.StatusCode);
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");

            // Trying the same request but without the If-Match header. The behavior in this case is "update or insert",
            // meaning that the server should try to insert the entity if it doesn't exist, so the answer we receive is
            // 201 (Created).
            request = new HttpRequestMessage(HttpMethod.Put, serverUrl + "/odata/Customers(30)");
            request.Content = new ObjectContent<Customer>(newCustomer, new JsonMediaTypeFormatter());
            Console.WriteLine("Trying to update a non existing customer without the If-Match header");
            Console.WriteLine();
            response = client.SendAsync(request).Result;
            Console.WriteLine("The response status code is {0}", response.StatusCode);
            Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------");
        }
    }
}
