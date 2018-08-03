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

        static void RunClient()
        {
            HttpClient client = new HttpClient();

            // Issue PUT request with a simple string content
            StringContent content = new StringContent("Hello World", Encoding.UTF8, "text/plain");
            client.PutAsync(_addres, content).ContinueWith(
                (putTask) =>
                {
                    if (putTask.IsCanceled)
                    {
                        return;
                    }
                    if (putTask.IsFaulted)
                    {
                        throw putTask.Exception;
                    }
                    HttpResponseMessage putResponse = putTask.Result;

                    putResponse.EnsureSuccessStatusCode();

                    RunGetRequest(client);
                });
        }

        static void RunGetRequest(HttpClient client)
        { 
            // Issue GET request to get the string back
            client.GetAsync(_addres).ContinueWith(
                (getTask) =>
                {
                    if (getTask.IsCanceled)
                    {
                        return;
                    }
                    if (getTask.IsFaulted)
                    {
                        throw getTask.Exception;
                    }
                    HttpResponseMessage getResponse = getTask.Result;
 
                    getResponse.EnsureSuccessStatusCode();
                    string result = getResponse.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Received response: {0}", result);
                });
        }

        static void Main(string[] args)
        {
            RunClient();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
