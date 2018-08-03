using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using ODataService.Extensions;

namespace ODataService.WebHost
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            // Add $format support
            config.MessageHandlers.Add(new FormatQueryMessageHandler());

            // Add NavigationRoutingConvention2 to support POST, PUT, PATCH and DELETE on navigation property
            var conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new CustomNavigationRoutingConvention());

            // Enables OData support by adding an OData route and enabling querying support for OData.
            // Action selector and odata media type formatters will be registered in per-controller configuration only
            config.Routes.MapODataServiceRoute(
                routeName: "OData",
                routePrefix: null,
                model: ModelBuilder.GetEdmModel(),
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions);

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            config.Filters.Add(new ModelValidationFilterAttribute());
        }
    }
}
