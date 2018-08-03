using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using Microsoft.Owin.Hosting;
using Owin;

namespace UploadXDocumentSample
{
    /// <summary>
    /// Sample uploading an XDocument using PushStreamContent and HttpClient.
    /// </summary>
    class Program
    {
        static readonly string _baseAddress = "http://localhost:50231";

        static void Main(string[] args)
        {
            IDisposable server = null;

            try
            {
                server = WebApp.Start<Program>(url: _baseAddress);

                Console.WriteLine("Listening on " + _baseAddress);

                // Run HttpClient issuing requests
                RunClient().Wait();

                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    // Stop listening
                    server.Dispose();
                }
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        
            appBuilder.UseWebApi(config);
        }

        static async Task RunClient()
        {
            // Create an HttpClient instance
            HttpClient client = new HttpClient();

            // Use chunked encoding as otherwise HttpClient would try buffering the content to 
            // figure out the content length.
            client.DefaultRequestHeaders.TransferEncodingChunked = true;

            // Create a push content so that we can use XDocument.Save to a stream
            XDocument xDoc = XDocument.Load("Sample.xml", LoadOptions.None);
            PushStreamContent xDocContent = new PushStreamContent(
                (stream, content, context) =>
                {
                    // After save we close the stream to signal that we are done writing.
                    using (stream)
                    {
                        xDoc.Save(stream);
                    }
                },
                "application/xml");

            // Send POST request to server and wait asynchronously for the response
            Uri address = new Uri(_baseAddress + "/api/book");
            HttpResponseMessage response = await client.PostAsync(address, xDocContent);

            // Ensure we get a successful response.
            response.EnsureSuccessStatusCode();

            // Read the response using XDocument as well
            Stream responseStream = await response.Content.ReadAsStreamAsync();
            XDocument xResponseDoc = XDocument.Load(responseStream);
            Console.WriteLine("Received response: {0}", xResponseDoc.ToString());
        }
    }
}