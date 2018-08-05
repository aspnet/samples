using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using TwitterSample.OAuth;

namespace TwitterSample
{
    /// <summary>
    /// Sample illustrating how to write a simple twitter client using HttpClient. The sample uses a 
    /// HttpMessageHandler to insert the appropriate OAuth authentication information into the outgoing
    /// HttpRequestMessage. The result from twitter is read as a JToken.
    /// </summary>
    /// <remarks>
    /// Before you can run this sample you must obtain an application key from twitter, and 
    /// fill in the information in the OAuthMessageHandler class, see 
    /// http://dev.twitter.com/ for details.
    /// </remarks>
    class Program
    {
        private static string _address = "https://api.twitter.com/1.1/statuses/user_timeline.json?include_rts=true&screen_name=scottgu&count=20";

        private static async void RunClient()
        {
            // Create client and insert an OAuth message handler in the message path that 
            // inserts an OAuth authentication header in the request
            HttpClient client = new HttpClient(new OAuthMessageHandler(new HttpClientHandler()));

            // Send asynchronous request to twitter and read the response as JToken
            HttpResponseMessage response = await client.GetAsync(_address);

            if (response.IsSuccessStatusCode)
            {
                JToken statuses = await response.Content.ReadAsAsync<JToken>();
                Console.WriteLine("Most recent statuses from ScottGu's twitter account:");
                Console.WriteLine();
                foreach (var status in statuses)
                {
                    Console.WriteLine("   {0}", status["text"]);
                    Console.WriteLine();
                }
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
