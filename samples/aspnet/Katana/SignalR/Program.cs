using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;

namespace SignalR
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:9999/";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started");
                // Open the SignalR negotiation page to make sure things are working.
                Process.Start(uri + "signalr/negotiate");
                Console.ReadKey();
                Console.WriteLine("Finished");
            }
        }
    }
}
