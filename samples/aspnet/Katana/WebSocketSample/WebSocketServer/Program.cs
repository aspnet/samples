using Microsoft.Owin.Hosting;
using System;

namespace WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:5000/"))
            {
                Console.WriteLine("Ready, press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
