using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MjrChess.Trainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specify wwwroot/dist as the web root since that's where
                    // webpack builds static assets to.
                    webBuilder.UseWebRoot("wwwroot/dist");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
