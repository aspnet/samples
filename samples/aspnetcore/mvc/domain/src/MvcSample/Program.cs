using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MvcSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.ListenLocalhost(5000);
                    options.ListenLocalhost(5001);
                    options.ListenLocalhost(5002);
                })
                .UseIISIntegration()
                .ConfigureLogging(b =>
                {
                    b.AddConsole();
                    b.SetMinimumLevel(LogLevel.Critical);
                })
                .UseContentRoot(Environment.CurrentDirectory)
                .UseStartup<Startup>();
        }
    }
}
