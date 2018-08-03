using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.ServiceModel;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ValidationSample.Models;

namespace ValidationSample
{
    /// <summary>
    /// This sample illustrates how you can use validation attributes on your models in ASP.NET WebAPI
    /// to validate the contents of the HTTP request. It demonstrates how to mark properties as required,
    /// how to use both framework-defined and custom validation attributes to annotate your model, and 
    /// how to return error responses for invalid model states.
    /// 
    /// For more information about this and other samples, please see 
    /// http://go.microsoft.com/fwlink/?LinkId=261487
    /// </summary>
    public class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");
        static readonly Uri _address = new Uri(_baseAddress, "/api/customer");

        public static void Main(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                // Set up server configuration
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;
                config.Formatters.JsonFormatter.Indent = true;
                config.Formatters.XmlFormatter.Indent = true;
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                // Create server
                server = new HttpSelfHostServer(config);

                // Start listening
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + _baseAddress);

                Console.WriteLine("*******************************");
                Console.WriteLine("POSTing a valid customer");
                Console.WriteLine("*******************************");
                PostValidCustomer();

                Console.WriteLine("*******************************");
                Console.WriteLine("POSTing a customer with a missing ID");
                Console.WriteLine("*******************************");
                PostCustomerMissingID();

                Console.WriteLine("*******************************");
                Console.WriteLine("POSTing a customer with negative ID and invalid phone number");
                Console.WriteLine("*******************************");
                PostInvalidCustomerUsingJson();

                Console.WriteLine("*******************************");
                Console.WriteLine("POSTing a customer with negative ID and invalid phone number using XML");
                Console.WriteLine("*******************************");
                PostInvalidCustomerUsingXml();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
            }
            finally
            {
                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
        }

        private static void PostValidCustomer()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _address)
            {
                Content = new ObjectContent<Customer>(
                    new Customer() { ID = 10, Name = "Steve", PhoneNumber = "(123) 456-7890" },
                    new JsonMediaTypeFormatter())
            };
            SubmitRequest(request);
        }

        private static void PostCustomerMissingID()
        {
            string body = @"{""Name"":""Steve"",""PhoneNumber"":""(123) 456-7890""}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _address)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            SubmitRequest(request);
        }

        private static void PostInvalidCustomerUsingJson()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _address)
            {
                Content = new ObjectContent<Customer>(
                    new Customer() { ID = -2, Name = "Steve", PhoneNumber = "23987928347" },
                    new JsonMediaTypeFormatter())
            };
            SubmitRequest(request);
        }

        private static void PostInvalidCustomerUsingXml()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _address)
            {
                Content = new ObjectContent<Customer>(
                    new Customer() { ID = -2, Name = "Steve", PhoneNumber = "23987928347" },
                    new XmlMediaTypeFormatter())
            };
            SubmitRequest(request);
        }

        /// <summary>
        /// Helper method to submit a request and print the request and response to the Console
        /// </summary>
        private static void SubmitRequest(HttpRequestMessage request)
        {
            Console.WriteLine("Submitting request:");
            Console.WriteLine(request);
            if (request.Content != null)
            {
                Console.WriteLine(request.Content.ReadAsStringAsync().Result);
            }

            // Create an HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Issue a request
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    Console.WriteLine("Request completed with response:");
                    Console.WriteLine(response);
                    if (response.Content != null)
                    {
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Request failed: {0}", e);
                }
            }
        }
    }
}
