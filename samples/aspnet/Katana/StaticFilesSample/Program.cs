using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;

namespace StaticFilesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:12345";
            using (WebApp.Start<Startup>(url))
            {
                Process.Start(url); // Launch the browser.
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
