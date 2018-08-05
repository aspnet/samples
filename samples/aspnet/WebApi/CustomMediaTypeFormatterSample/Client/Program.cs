using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Client
{
    /// <summary>
    /// This sample client sends a request with an accept header saying "Accept: text/plain".
    /// The response should come back in plain text using the custom text media type formatter
    /// registered on the server.
    /// </summary>
    class Program
    {
        static readonly Uri _address = new Uri("http://localhost:51972/api/values/0");

        static void RunClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));

            // Issue GET request
            client.GetAsync(_address).ContinueWith(
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
                    Console.WriteLine("Received response formatted using the plain/text media type formatter: {0}", result);
                    Console.WriteLine("Content headers: \n{0}", getResponse.Content.Headers);
                });

            // Issue PUT request of string content
            StringContent content = new StringContent("Hello World", Encoding.UTF8, "text/plain");
            client.PutAsync(_address, content).ContinueWith(
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
