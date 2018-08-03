using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// This sample client tries out the ValuesController exposed by the WebForm project in this solution.
    /// It issues a GET request to retrieve the list of values and displays the results on screen.
    /// </summary>
    class Program
    {
        static readonly Uri _address = new Uri("http://localhost:65291/api/values");

        static async Task RunClient()
        {
            HttpClient client = new HttpClient();

            // Issue GET request against Web API
            HttpResponseMessage getResponse = await client.GetAsync(_address);
            getResponse.EnsureSuccessStatusCode();

            string result = getResponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Received response: {0}", result);
        }

        static void Main(string[] args)
        {
            RunClient().Wait();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
