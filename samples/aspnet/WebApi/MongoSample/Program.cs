using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using MongoSample.Models;

namespace MongoSample
{
    /// <summary>
    /// This sample illustrates how to use MongoDB as the persistent store using a repository pattern.
    /// To get started with MongoDB, please see http://www.mongodb.org/display/DOCS/Quickstart+Windows
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;
            try
            {
                // Set up server configuration
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
                config.HostNameComparisonMode = HostNameComparisonMode.Exact;

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                // Create server
                server = new HttpSelfHostServer(config);

                // Start listening
                server.OpenAsync().Wait();

                // Run HttpClient issuing requests
                RunClient();

                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
            }
        }

        /// <summary>
        /// Runs an HttpClient issuing a GET and a POST request against the controller.
        /// The controller also supports PUT and DELETE.
        /// </summary>
        static async void RunClient()
        {
            HttpClient client = new HttpClient();
            Uri address = new Uri(_baseAddress, "/api/contacts");

            // Get the 1st entry
            HttpResponseMessage response1 = await client.GetAsync(address);

            // Check that response was successful or throw exception
            response1.EnsureSuccessStatusCode();

            // Read response asynchronously as string and write out
            IEnumerable<Contact> content1 = await response1.Content.ReadAsAsync<IEnumerable<Contact>>();

            Console.WriteLine("Got the following entries from controller:");
            foreach (Contact contact in content1)
            {
                PrintContact(contact);
            }

            // Post a new entry and show result
            HttpResponseMessage response2 = await client.PostAsJsonAsync(address.ToString(),
                new Contact
                {
                    Email = "new@example.com",
                    Name = "new",
                    Phone = "000 000 0000",
                });

            // Check that response was successful or throw exception
            response2.EnsureSuccessStatusCode();

            // Read response asynchronously as string and write out result
            Contact content2 = await response2.Content.ReadAsAsync<Contact>();
            Console.WriteLine("\nAdded entry:\n");
            PrintContact(content2);
        }

        private static void PrintContact(Contact contact)
        {
            Console.WriteLine("  Email:         {0}", contact.Email);
            Console.WriteLine("  Name:          {0}", contact.Name);
            Console.WriteLine("  Phone:         {0}", contact.Phone);
            Console.WriteLine("  ID:            {0}", contact.Id);
            Console.WriteLine("  Last Modified: {0}", contact.LastModified);
            Console.WriteLine();
        }
    }
}
