using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace WorldBankSample
{
    /// <summary>
    /// Sample download list of countries from the World Bank Data sources at http://data.worldbank.org/
    /// </summary>
    class Program
    {
        private static string _address = "http://api.worldbank.org/countries?format=json";

        private static async void RunClient()
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Send a request asynchronously and continue when complete
            HttpResponseMessage response = await client.GetAsync(_address);

            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // Read response asynchronously as JToken and write out top facts for each country
            JArray content = await response.Content.ReadAsAsync<JArray>();

            Console.WriteLine("First 50 countries listed by The World Bank...");
            foreach (var country in content[1])
            {
                Console.WriteLine("   {0}, Country Code: {1}, Capital: {2}, Latitude: {3}, Longitude: {4}",
                    country.Value<string>("name"),
                    country.Value<string>("iso2Code"),
                    country.Value<string>("capitalCity"),
                    country.Value<string>("latitude"),
                    country.Value<string>("longitude"));
            }
        }

        static void Main(string[] args)
        {
            RunClient();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
