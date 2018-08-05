using System.Web.Http;
using System.Web.Http.Routing;
using RouteConstraintsAndModelBindersSample.Server.RouteConstraints;

namespace RouteConstraintsAndModelBindersSample.Server
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Register the SegmentPrefixConstraint for matching an exact segment prefix.
            DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("SegmentPrefix", typeof(SegmentPrefixConstraint));

            // Web API routes
            config.MapHttpAttributeRoutes(constraintResolver);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
