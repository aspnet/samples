using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace ODataService.SelfHost
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");
        static void Main(string[] args)
        {
            // Start listening
            using (WebApp.Start<Startup>(url: _baseAddress.OriginalString))
            {
                Console.WriteLine("Listening on {0}. Press Enter to Exit.", _baseAddress);
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Set up server configuration
            var config = new HttpConfiguration() { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };

            // Enables OData support by adding an OData route and enabling querying support for OData.
            // Action selector and odata media type formatters will be registered in per-controller configuration only
            config.MapODataServiceRoute(
                routeName: "OData",
                routePrefix: null,
                model: ModelBuilder.GetEdmModel());

            appBuilder.UseWebApi(config);
            
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
    }
}
