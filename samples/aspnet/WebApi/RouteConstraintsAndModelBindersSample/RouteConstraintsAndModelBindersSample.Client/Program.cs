using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RouteConstraintsAndModelBindersSample.Client
{
    public class Program
    {
        private const string ServerAddress = "http://localhost:1427/";

        public static void Main()
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            Console.WriteLine("Press ENTER to start . . .");
            Console.ReadLine();
            Console.WriteLine();

            // Each following Get*From* method is matched with an action
            // in the \RouteConstraintsAndModelBindersSample.Server\FruitsController.cs
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("========================================");
                await GetFruitsFromLocation(client);
                Console.WriteLine("========================================");

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("========================================");
                await GetApplesFromWashington(client);
                Console.WriteLine("========================================");

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("========================================");
                await GetApplesFromLocation(client);
                Console.WriteLine("========================================");

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("========================================");
                await GetAttributesFromOptionalSegments(client);
                Console.WriteLine("========================================");
            }

            Console.WriteLine("Press ENTER to exit . . .");
            Console.ReadLine();
        }

        private static async Task GetFruitsFromLocation(HttpClient client)
        {
            Console.WriteLine("GetFruitsFromLocation");
            Console.WriteLine();

            const string noMatrixParam = "customers/2/oranges/connecticut";
            await RequestAndPrintAsync(client, relativePath: noMatrixParam);

            const string variant1 = "customers/2/bananas;color=yellow;rate=good;color=green;rate=excellent/oregon";
            await RequestAndPrintAsync(client, relativePath: variant1);

            const string variant2 = "customers/2/limes/california;color=green,red;rate=excellent,good";
            await RequestAndPrintAsync(client, relativePath: variant2);
        }

        private static async Task GetApplesFromWashington(HttpClient client)
        {
            Console.WriteLine("GetApplesFromWashington");
            Console.WriteLine();

            const string noMatrixParam = "customers/2/apples/washington";
            await RequestAndPrintAsync(client, relativePath: noMatrixParam);

            const string variant1 = "customers/2/apples;color=red;rate=good;color=green;rate=excellent/washington";
            await RequestAndPrintAsync(client, relativePath: variant1);

            const string variant2 = "customers/2/apples;color=red;rate=good/washington;color=green;rate=excellent";
            await RequestAndPrintAsync(client, relativePath: variant2);

            const string variant3 = "customers/2/apples/washington;color=green,red;rate=excellent,good";
            await RequestAndPrintAsync(client, relativePath: variant3);
        }

        private static async Task GetApplesFromLocation(HttpClient client)
        {
            Console.WriteLine("GetApplesFromLocation");
            Console.WriteLine();

            const string noMatrixParam = "customers/2/APPLES/connecticut"; // Ignore case for matching route prefix.
            await RequestAndPrintAsync(client, relativePath: noMatrixParam);

            const string variant1 = "customers/2/apples;color=red;rate=good;color=green;rate=excellent/connecticut";
            await RequestAndPrintAsync(client, relativePath: variant1);

                                                                                       // Ignore case for param name.
            const string variant2 = "customers/2/apples;color=red;rate=good/california;COLOR=green;rate=excellent";
            await RequestAndPrintAsync(client, relativePath: variant2);

            const string variant3 = "customers/2/apples/california;color=green,red;rate=excellent,good";
            await RequestAndPrintAsync(client, relativePath: variant3);
        }

        private static async Task GetAttributesFromOptionalSegments(HttpClient client)
        {
            Console.WriteLine("GetAttributesFromOptionalSegments");
            Console.WriteLine();

            const string noMatrixParam = "customers/2/optional/foo/oranges/bar/connecticut";
            await RequestAndPrintAsync(client, relativePath: noMatrixParam);

            const string variant1 = "customers/2/optional/apples;color=red,green;rate=excellent/oregon/bar";
            await RequestAndPrintAsync(client, relativePath: variant1);

            const string variant2 = "customers/2/optional/foo;color=yellow/limes/california;/bar/color=green,red;rate=excellent,good";
            await RequestAndPrintAsync(client, relativePath: variant2);
        }

        private static async Task RequestAndPrintAsync(HttpClient client, string relativePath)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ServerAddress + relativePath))
            {
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    Console.WriteLine("{0} {1}", (int)response.StatusCode, response.ReasonPhrase);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return;
                    }

                    string requestUri = response.RequestMessage.RequestUri.ToString();
                    Console.WriteLine("RequestUri: " + requestUri);

                    Console.WriteLine("Matched Values of Matrix Parameters: ");
                    JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
                    Console.WriteLine(json.ToString(Formatting.Indented));
                    Console.WriteLine();
                }
            }
        }
    }
}
