// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomActionSelector.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "http://localhost:27209";

            Console.WriteLine("Listening on: " + address);
            Console.WriteLine();

            Console.WriteLine("Press ENTER to start fetching data.");
            Console.WriteLine();
            Console.ReadLine();

            GetProducts(address).Wait();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async Task GetProducts(string address)
        {
            var uriBuilder = new UriBuilder(address);
            uriBuilder.Path = "api/Store";

            using (var client = new HttpClient())
            using (var betaClient = new HttpClient())
            {
                betaClient.DefaultRequestHeaders.Add("beta-tester", Boolean.TrueString);

                Console.WriteLine("Getting all data for a normal user");
                var response = await client.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();
                await PrintResponse(response);

                Console.WriteLine("Getting all data for a beta testing user");
                for (int i = 0; i < 3; i++)
                {
                    response = await betaClient.GetAsync(uriBuilder.Uri);
                    response.EnsureSuccessStatusCode();
                    await PrintResponse(response);

                    await Task.Delay(750);
                }

                uriBuilder.Path = "api/Store/2";
                Console.WriteLine("Getting a product's data for a normal user");
                response = await client.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();
                await PrintResponse(response);

                Console.WriteLine("Getting a product's data for a beta testing user");
                for (int i = 0; i < 3; i++)
                {
                    response = await betaClient.GetAsync(uriBuilder.Uri);
                    response.EnsureSuccessStatusCode();
                    await PrintResponse(response);

                    await Task.Delay(750);
                }
            }
        }

        private static async Task PrintResponse(HttpResponseMessage response)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            IEnumerable<string> headers;
            if (response.Headers.TryGetValues("x-beta-action", out headers))
            {
                if (headers.Any())
                {
                    Console.WriteLine("x-beta-action: " + headers.First());
                }
                else
                {
                    Console.WriteLine("x-beta-action: (not set)");
                }
            }
            else
            {
                Console.WriteLine("x-beta-action: (not set)");
            }

            Console.WriteLine();
        }
    }
}
