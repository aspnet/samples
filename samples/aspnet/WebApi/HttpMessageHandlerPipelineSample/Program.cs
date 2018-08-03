using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace HttpMessageHandlerPipelineSample
{
    /// <summary>
    /// This sample illustrates how to wire up HttpMessageHandlers on both client and server side
    /// as part of either HttpClient or ASP.NET Web API.
    /// In the sample, the same handler is used on both client and server side. While it is rare
    /// that the exact same handler can run in both places, the object model is the same on 
    /// client and server side.
    ///
    /// For detailed information about this sample, please see
    /// http://blogs.msdn.com/b/henrikn/archive/2012/08/07/httpclient-httpclienthandler-and-httpwebrequesthandler.aspx
    ///
    /// For more information about this and other samples, please see 
    /// http://go.microsoft.com/fwlink/?LinkId=261487
    /// </summary>
    class Program
    {
        static readonly string _baseAddress = "http://localhost:50231";
        static readonly Uri _address = new Uri(_baseAddress + "/api/sample");

        static void Main(string[] args)
        {
            IDisposable server = null;
            try
            {
                server = WebApp.Start<Program>(url: _baseAddress);

                Console.WriteLine("Listening on " + _baseAddress);

                // Run HttpClient issuing requests in various combinations
                RunHttpClientAsync().Wait();

                RunHttpClientWithMultipleDelegatingHandlersAsync().Wait();

                RunHttpClientWithHttpClientHandler().Wait();

                RunHttpClientWithHttpClientHandlerAndMultipleDelegatingHandlersAsync().Wait();

                RunHttpClientWithWebRequestHandlerAsync().Wait();

                RunHttpClientWithWebReqeustHandlerAndMultipleDelegatingHandlersAsync().Wait();

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

            // Add our message handlers for the server
            config.MessageHandlers.Add(new SampleHandler("Server A", 2));
            config.MessageHandlers.Add(new SampleHandler("Server B", 4));
            config.MessageHandlers.Add(new SampleHandler("Server C", 6));

            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Basic use of HttpClient
        /// </summary>
        static async Task RunHttpClientAsync()
        {
            Console.WriteLine("Issue request using a basic HttpClient.");

            // Create an HttpClient and set the timeout for requests
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            // Issue a request
            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }

        /// <summary>
        /// HttpClient using HttpClientHandler
        /// </summary>
        static async Task RunHttpClientWithHttpClientHandler()
        {
            Console.WriteLine("Issue request using an HttpClient with HttpClientHandler.");

            // Create HttpClientHandler and set UseDefaultCredentials property
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = true;

            // Create an HttpClient using the HttpClientHandler
            HttpClient client = new HttpClient(clientHandler);

            // Issue a request
            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }

        /// <summary>
        /// HttpClient using WebRequestHandler
        /// </summary>
        static async Task RunHttpClientWithWebRequestHandlerAsync()
        {
            Console.WriteLine("Issue request using an HttpClient with WebRequestHandler.");

            // Create WebRequestHandler and set UseDefaultCredentials and AllowPipelining (for HTTP) properties
            WebRequestHandler webRequestHandler = new WebRequestHandler();
            webRequestHandler.UseDefaultCredentials = true;
            webRequestHandler.AllowPipelining = true;

            // Create an HttpClient using the WebRequestHandler
            HttpClient client = new HttpClient(webRequestHandler);

            // Issue a request
            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }

        /// <summary>
        /// Wire up three sample HttpMessageHandler instances to the client which means
        /// that each request and response will go through these three before hitting the
        /// network.
        /// </summary>
        static async Task RunHttpClientWithMultipleDelegatingHandlersAsync()
        {
            Console.WriteLine("Issue request using an HttpClient with multiple delegating handlers in the pipeline.");

            // Create an HttpClient and add message handlers for the client
            HttpClient client = HttpClientFactory.Create(
                new SampleHandler("Client A", 2),
                new SampleHandler("Client B", 4),
                new SampleHandler("Client C", 6));

            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }

        /// <summary>
        /// Wire up three sample HttpMessageHandler instances to the client which means
        /// that each request and response will go through these three before hitting the
        /// network.
        /// </summary>
        static async Task RunHttpClientWithHttpClientHandlerAndMultipleDelegatingHandlersAsync()
        {
            Console.WriteLine("Issue request using an HttpClient with HttpClientHandler and multiple delegating handlers in the pipeline.");

            // Create HttpClientHandler and set UseDefaultCredentials property
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = true;

            // Create an HttpClient and add message handlers for the client
            HttpClient client = HttpClientFactory.Create(
                clientHandler,
                new SampleHandler("Client A", 2),
                new SampleHandler("Client B", 4),
                new SampleHandler("Client C", 6));

            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }

        /// <summary>
        /// Wire up three sample HttpMessageHandler instances to the client which means
        /// that each request and response will go through these three before hitting the
        /// network.
        /// </summary>
        static async Task RunHttpClientWithWebReqeustHandlerAndMultipleDelegatingHandlersAsync()
        {
            Console.WriteLine("Issue request using an HttpClient with WebRequestHandler and multiple delegating handlers in the pipeline.");

            // Create WebRequestHandler and set UseDefaultCredentials and AllowPipelining (for HTTP) properties
            WebRequestHandler webRequestHandler = new WebRequestHandler();
            webRequestHandler.UseDefaultCredentials = true;
            webRequestHandler.AllowPipelining = true;

            // Create an HttpClient and add message handlers for the client
            HttpClient client = HttpClientFactory.Create(
                webRequestHandler,
                new SampleHandler("Client A", 2),
                new SampleHandler("Client B", 4),
                new SampleHandler("Client C", 6));

            HttpResponseMessage response = await client.GetAsync(_address);
            Console.WriteLine("Request completed with status code {0}", response.StatusCode);
        }
    }
}
