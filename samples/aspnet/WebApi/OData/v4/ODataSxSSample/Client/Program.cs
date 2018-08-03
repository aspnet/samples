
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunAsync().Wait();
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }

        static async Task RunAsync()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:12374/")
            };

            try
            {
                // Access the V1 service without any version query string.
                var response = await client.GetAsync("/odata/Orders/");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nOrder list from the V1 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V1 service with the V1 version query string.
                response = await client.GetAsync("/odata/Products(1)/?v=1");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nSingle product from the V1 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V1 metadata without any version query string.
                response = await client.GetAsync("/odata/$metadata");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nMetadata from the V1 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V1 metadata with the V1 version query string.
                response = await client.GetAsync("/odata/$metadata?v=1");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nMetadata from the V1 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V1 service with the V1 version query string; select the first 2 orders.
                response = await client.GetAsync("/odata/Orders/?v=1&$top=2");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("\nShortened order list from the V1 service returned: " +
                await response.Content.ReadAsStringAsync());

                // Access the V1 service with the V1 version query string; select the products and inline count.
                response = await client.GetAsync("/odata/Products/?v=1&$inlinecount=allpages");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("\nCounted product list from the V1 service returned: " +
                await response.Content.ReadAsStringAsync());

                // Access the V2 service with the V2 version query string.
                // The orders service in V2 uses the OData Attribute Routing when selecting actions.
                response = await client.GetAsync("/odata/Orders(1)/?v=2");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nSingle order from the V2 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V2 service with the V2 version query string.
                // The products service in V2 rely on the traditional convention when selecting actions. 
                response = await client.GetAsync("/odata/Products(1)/?v=2");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nSingle product from the V2 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V2 metadata with the V2 version query string.
                response = await client.GetAsync("/odata/$metadata?v=2");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(
                    "\nMetadata from the V2 service returned: " + await response.Content.ReadAsStringAsync());

                // Access the V2 service with the V2 version query string; select the first 2 orders.
                response = await client.GetAsync("/odata/Orders/?v=2&$top=2");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("\nShortened order list from the V2 service returned: " +
                await response.Content.ReadAsStringAsync());

                // Access the V2 service with the V2 version query string; select the products and inline count.
                response = await client.GetAsync("/odata/Products/?v=2&$count=true");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("\nCounted product list from the V2 service returned: " +
                await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
