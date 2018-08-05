using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Tracing;
using WebApiEtwTracing;

namespace ConsoleDemo
{
    /// <summary>
    /// Console app demonstrating how the WebAPIEtwTraceWriter can be used.
    /// More details are available in the ReadMe.txt for this solution.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            string outFile = null;
            if (args.Length > 0 && !String.IsNullOrEmpty(args[0]))
            {
                outFile = Path.GetFullPath(args[0]);
                if (File.Exists(outFile))
                {
                    File.Delete(outFile);
                }

                // This starts a process to request logman to start listening for
                // the WebApiEtwEventSource using its Guid.
                Console.WriteLine("Starting logman to write the log to " + outFile);
                string logmanArgs = "start WebApi -rt -nb 2 2 -bs 1024 -p {fc7bbb67-1b01-557b-5e0e-c1c4e40b6a24} 0xffffffffffffffff 0x5 -ets -o \"" + outFile + "\"";
                System.Diagnostics.Process.Start("logman.exe", logmanArgs);
            }
            else
            {
                Console.WriteLine("To produce an .etl file using this app, use 'ConsoleDemo.exe myFile.etl'.");
                Console.WriteLine("Alternatively, you can run ConsoleDemo.exe (no arguments) from PerfView.");
                Console.WriteLine("(add *WebApi to the Additional Providers before you run it from PerfView).");
            }

            Console.WriteLine();

            // The following code demonstrates how WebApiEtwTraceWriter works
            // by sending client Http requests to an in-memory HttpServer,
            // causing the WebApi core code to trace its activities.  

            // Step 1: all WebApi apps are configured via HttpConfiguration.
            HttpConfiguration config = new HttpConfiguration();

            // Step 2: create a WebApiEtwTraceWriter and register it
            // with the WebApi configuration's services.  This registration
            // effectively enables tracing from the WebAPI core.
            WebApiEtwTraceWriter traceWriter = new WebApiEtwTraceWriter();
            config.Services.Replace(typeof(ITraceWriter), traceWriter);

            // Uncomment the next 2 lines to remove all filtering and to
            // allow every trace request to trace to ETW.
            // The default filters permit only a small
            // subset of traces to reduce the noise.
            // Alternatively, provide your own predicates here to
            // control which trace requests are sent to ETW.

            //traceWriter.TraceRequestFilter = null;
            //traceWriter.TraceRecordFilter = null;

            // Step 3: create a route so that the Http GET requests are
            // directed by WebApi to the TestController.
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = "1", controller = "Test" });

            // Step 4: ask to see all error details, including stack traces.
            // This step is for demonstration purposes.  Do not do this in a
            // production environment or you will disclose runtime information
            // to the client.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Step 5: create an in-memory HttpServer.
            // This approach allows this app to exercise the WebAPI
            // stack without requiring any network traffic.
            using (HttpServer server = new HttpServer(config))
            {
                // Step 6: create an HttpClient to issue client Http requests.
                // By connecting it directly to the HttpServer, these
                // requests are handled without using a network.
                using (HttpClient client = new HttpClient(server))
                {
                    // Step 7: send a series of Http GET requests.
                    // Each request will cause WebAPI to trace to ETW.
                    // Each is slightly different to demonstrate different
                    // success and error paths through WebAPI.
                    HttpRequestMessage request;
                    HttpResponseMessage response;

                    // Generate a valid GET with an explicit id
                    Console.WriteLine("    Issuing a legal GET to http://localhost/api/Test?id=1 ...");
                    request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Test?id=1");
                    response = client.SendAsync(request).Result;
                    Assert(response, HttpStatusCode.OK);

                    // Generate a valid GET with a defaulted id
                    Console.WriteLine("    Issuing a legal GET to http://localhost/api/Test ...");
                    request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Test");
                    response = client.SendAsync(request).Result;
                    Assert(response, HttpStatusCode.OK);

                    // Issue an invalid GET that causes a model binding error.
                    Console.WriteLine("    Issuing a model-binding error GET to http://localhost/api/Test?id=x ...");
                    request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Test?id=x");
                    response = client.SendAsync(request).Result;
                    Assert(response, HttpStatusCode.BadRequest);

                    // Issue a GET that causes a divide by zero in user code.
                    Console.WriteLine("    Issuing a GET to http://localhost/api/Test?id=0 to trigger divide by zero in user code ...");
                    request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Test?id=0");
                    response = client.SendAsync(request).Result;
                    Assert(response, HttpStatusCode.InternalServerError);

                    // Issue a GET to an unrecognized endpoint.
                    Console.WriteLine("    Issuing a invalid GET to http://localhost/api/Testt ...");
                    request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/Testt");
                    response = client.SendAsync(request).Result;
                    Assert(response, HttpStatusCode.NotFound);
                }

                if (outFile != null)
                {
                    // This asks logman to stop listening to the WebApiEtwEventSource and
                    // to write its traces to the output file.
                    Console.WriteLine("Stopping logman tracing...");
                    System.Diagnostics.Process.Start("logman.exe", "stop WebApi -ets").WaitForExit();

                    Console.WriteLine("The WebApi log file is: " + outFile);
                    Console.WriteLine("The WebApi manifest is WebApiEtwTracing.man in the root solution folder");
                }

                Console.WriteLine("Done.");
            }
        }

        private static void Assert(HttpResponseMessage response, HttpStatusCode status)
        {
            if (response.StatusCode != status)
            {
                throw new InvalidOperationException(String.Format("Expected {0} but received {1}.", status, response.StatusCode));
            }

            Thread.Sleep(100);
        }
    }
}
