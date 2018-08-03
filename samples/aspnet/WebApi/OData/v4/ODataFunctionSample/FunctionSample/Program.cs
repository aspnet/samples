using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace FunctionSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:9010/";
            var client = new HttpClient();

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Comment("Service is listening to " + baseAddress);

                Comment("List all the products.");

                Comment(client.GetAsync(baseAddress + "odata/Products").Result.Content.ReadAsStringAsync().Result);
                Comment();

                // Sample 1: return a value from a function bound to a collection
                ExecuteSample("Sample 1: Get the most expensive product.",
                    baseAddress + "odata/Products/Default.MostExpensive()").Wait(1000);
                // Sample 2: return a collection from a function bound to a collection
                ExecuteSample("Sample 2: Get the top 10 expensive product.",
                    baseAddress + "odata/Products/Default.Top10()").Wait(1000);
                // Sample 3: return a value from a function bound to an entity
                ExecuteSample("Sample 3: Get the rank of the product price.",
                    baseAddress + "odata/Products(33)/Default.GetPriceRank()").Wait(1000);
                // Sample 4: return a value from a function which accepts a parameter
                ExecuteSample("Sample 4: Get the sales tax.",
                    baseAddress + "odata/Products(33)/Default.CalculateGeneralSalesTax(state='WA')").Wait(1000);
                // Sample 5: return a value from a services level function
                ExecuteSample("Sample 5: Get the sales tax rate.",
                    baseAddress + "odata/GetSalesTaxRate(state='CA')").Wait(1000);
                //Sample 6: call function with parameter alias
                ExecuteSample("Sample 6: call function with parameter alias.",
                    baseAddress + "odata/GetSalesTaxRate(state=@p1)?@p1='ND'").Wait(1000);

                Comment("Press and key to exit.");
                Console.ReadKey();
            }
        }

        private static async Task ExecuteSample(string caseName, string url)
        {
            Comment(caseName);

            Comment("GET " + url);
            Comment();

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            Comment(response.ToString());
            Comment(content);
            Comment();
        }

        public static void Comment()
        {
            Console.WriteLine();
        }

        public static void Comment(string message)
        {
            Console.WriteLine(message);
        }
    }
}
