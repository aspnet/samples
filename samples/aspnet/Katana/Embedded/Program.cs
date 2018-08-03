using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;

namespace Embedded
{
    public class Program
    {
        // Shows how to launch an OWIN HTTP server in your own exe.
        public static void Main(string[] args)
        {
            string baseUrl = "http://localhost:12345/";

            using (WebApp.Start<Startup>(new StartOptions(baseUrl) { ServerFactory = "Microsoft.Owin.Host.HttpListener" }))
            {
                // Launch the browser
                Process.Start(baseUrl);

                // Keep the server going until we're done
                Console.WriteLine("Press Any Key To Exit");
                Console.ReadKey();
            }
        }
    }
}
