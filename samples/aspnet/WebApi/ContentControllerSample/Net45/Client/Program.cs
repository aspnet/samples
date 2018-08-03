using System;
using System.Net.Http;
using System.Text;

namespace Client
{
    /// <summary>
    /// This sample client tries out the ContentController exposed by the ContentController project in this solution.
    /// It first issues a PUT request and then a GET request.
    /// </summary>
    class Program
    {
        static readonly Uri _addres = new Uri("http://localhost:50232/api/content");

        static async void RunClient()
        {
            HttpClient client = new HttpClient();

            // Issue PUT request with a simple string content
            StringContent content = new StringContent("Hello World", Encoding.UTF8, "text/plain");
            HttpResponseMessage putResponse = await client.PutAsync(_addres, content);

            putResponse.EnsureSuccessStatusCode();

            // Issue GET request to get the string back
            HttpResponseMessage getResponse = await client.GetAsync(_addres);

            getResponse.EnsureSuccessStatusCode();
            string result = await getResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Received response: {0}", result);
        }

        static void Main(string[] args)
        {
            RunClient();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
