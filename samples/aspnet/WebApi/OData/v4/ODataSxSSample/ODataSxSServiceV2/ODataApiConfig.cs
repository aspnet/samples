using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using ODataSxSServiceV2.Extensions;
using ODataSxSServiceV2.Models;

namespace ODataSxSServiceV2
{
    public static class ODataApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Database.SetInitializer(new DatabaseInitialize());
            var model = ModelBuilder.GetEdmModel();

            // Wrap most routing conventions to redirect requests for e.g. /Products/... to the ProductsV2Controller.
            var defaultConventions = ODataRoutingConventions.CreateDefaultWithAttributeRouting(config, model);
            var conventions = new List<IODataRoutingConvention>();
            foreach (var convention in defaultConventions)
            {
                if (convention is MetadataRoutingConvention ||
                    convention is AttributeRoutingConvention)
                {
                    // Don't need to special case these conventions.
                    conventions.Add(convention);
                }
                else
                {
                    conventions.Add(new VersionedRoutingConvention(convention, "V2"));
                }
            }

            var odataRoute = config.MapODataServiceRoute(
                routeName: "odataV2",
                routePrefix: "odata",
                model: model,
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions);

            var constraint = new ODataVersionRouteConstraint(new { v = "2" });
            odataRoute.Constraints.Add("VersionConstraintV2", constraint);
        }
    }
}
