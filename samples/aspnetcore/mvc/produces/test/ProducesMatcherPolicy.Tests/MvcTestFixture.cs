using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace ProducesMatcherPolicy.Tests
{
    public class MvcTestFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Production")
                .ConfigureServices(
                    services =>
                    {
                        //var testSink = new TestSink();
                        //var loggerFactory = new TestLoggerFactory(testSink, enabled: true);
                        //services.AddSingleton<ILoggerFactory>(loggerFactory);
                    });
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-GB");
                CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                return base.CreateServer(builder);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }

}