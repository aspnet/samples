using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace ODataService.WebHost
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            // Enables OData support by adding an OData route and enabling querying support for OData.
            // Action selector and odata media type formatters will be registered in per-controller configuration only
            config.MapODataServiceRoute(
              routeName: "OData",
              routePrefix: null,
              model: ModelBuilder.GetEdmModel());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
