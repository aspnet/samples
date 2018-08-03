using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using ODataSxSService.Extensions;
using ODataSxSService.Models;

namespace ODataSxSService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Database.SetInitializer(new DatabaseInitialize());

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var odataRoute = config.Routes.MapODataServiceRoute(
                routeName: "odata",
                routePrefix: "odata",
                model: ModelBuilder.GetEdmModel()); 

            var contraint = new ODataVersionRouteConstraint(new { v = "1" });
            odataRoute.Constraints.Add("VersionContraintV1", contraint);

            ODataSxSServiceV2.ODataApiConfig.Register(config);
        }
    }
}
