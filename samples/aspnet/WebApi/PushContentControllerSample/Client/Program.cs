using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// This sample client reads a never-ending HTTP response sent from the
    /// <see cref="PushContentController"/> and prints the output to the console.
    /// </summary>
    class Program
    {
        static readonly Uri _address = new Uri("http://localhost:50232/api/pushcontent");

        static async Task RunClientAsync()
        {
            HttpClient client = new HttpClient();

            // Here we are using HttpCompletionOption.ResponseHeadersRead to only read headers
            // as by default HttpClient would buffer the response content. Since in this scenario
            // we want to get the individual updates from the server and print a friendly message
            // on the console, we want to read the response stream overselves, otherwise the 'GetAsync'
            // call would return only after all the response content is buffered, which could eventually
            // cause OutOfMemoryException.  
            HttpResponseMessage response = await client.GetAsync(_address, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            // Close the response stream here as we have explicitly said to HttpClient to read
            // the response stream ourselves.
            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                // Read response stream
                await ReadResponseStreamAsync(responseStream);
            }
        }

        private static async Task ReadResponseStreamAsync(Stream responseStream)
        {
            byte[] readBuffer = new byte[512];

            int bytesRead;
            while ((bytesRead = await responseStream.ReadAsync(readBuffer, 0, readBuffer.Length)) != 0)
            {
                string content = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);

                Console.WriteLine("Received: {0}", content);
            }
        }

        static void Main(string[] args)
        {
            RunClientAsync().Wait();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
