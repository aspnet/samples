using Microsoft.OData.Edm;
using ODataVersioningSample.Extensions;
using System.Collections.Generic;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using System.Web.Http.Routing;

namespace System.Web.Http
{
    public static class ODataVersionRouteExtensions
    {
        /// <summary>
        /// Map odata route with query string or header constraints
        /// </summary>
        public static void MapODataServiceRoute(
            this HttpRouteCollection routes,
            string routeName,
            string routePrefix,
            IEdmModel model,
            object queryConstraints,
            object headerConstraints)
        {
            MapODataServiceRoute(
                routes,
                routeName,
                routePrefix,
                model,
                new DefaultODataPathHandler(),
                ODataRoutingConventions.CreateDefault(),
                queryConstraints,
                headerConstraints);
        }

        /// <summary>
        /// Map odata route with query string or header constraints
        /// </summary>
        public static void MapODataServiceRoute(
            this HttpRouteCollection routes,
            string routeName,
            string routePrefix,
            IEdmModel model,
            IODataPathHandler pathHandler,
            IEnumerable<IODataRoutingConvention> routingConventions,
            object queryConstraints,
            object headerConstraints)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            string routeTemplate = string.IsNullOrEmpty(routePrefix) ? ODataRouteConstants.ODataPathTemplate : (routePrefix + "/" + ODataRouteConstants.ODataPathTemplate);
            ODataVersionRouteConstraint routeConstraint = new ODataVersionRouteConstraint(pathHandler, model, routeName, routingConventions, queryConstraints, headerConstraints);
            var constraints = new HttpRouteValueDictionary();
            constraints.Add(ODataRouteConstants.ConstraintName, routeConstraint);
            routes.MapHttpRoute(
                routeName,
                routeTemplate,
                defaults: null,
                constraints: constraints);
        }
    }
}