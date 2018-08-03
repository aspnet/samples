// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoutingConstraints.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "http://localhost:49980";

            Console.WriteLine("Listening on: " + address);
            Console.WriteLine();

            Console.WriteLine("Press ENTER to start fetching data.");
            Console.WriteLine();
            Console.ReadLine();

            GetPeople(address).Wait();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async Task GetPeople(string address)
        {
            var uriBuilder = new UriBuilder(address);
            uriBuilder.Path = "api/Customer";

            using (var client = new HttpClient())
            {
                Console.WriteLine("Getting data without a version...");
                var response = await client.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();

                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine();

                Console.WriteLine("Getting data for v1...");
                client.DefaultRequestHeaders.Add("api-version", "1");
                response = await client.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();

                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine();


                Console.WriteLine("Getting data for v2...");
                client.DefaultRequestHeaders.Remove("api-version");
                client.DefaultRequestHeaders.Add("api-version", "2");
                response = await client.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();

                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine();
            }
        }
    }
}
