using Microsoft.Data.Edm;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using System.Web.Http.Routing;

namespace ODataVersioningSample.Extensions
{
    /// <summary>
    /// Route constraint to allow constraint odata route by query string or headers
    /// which is partically used in versioning scenario. For example, you may set 
    /// query string constraint to v=1 and the route that matching it will be 
    /// considered as a v1 request and corresponding model will be used to server 
    /// it
    /// </summary>
    public class ODataVersionRouteConstraint : ODataPathRouteConstraint
    {
        public ODataVersionRouteConstraint(
            IODataPathHandler pathHandler,
            IEdmModel model,
            string routeName,
            IEnumerable<IODataRoutingConvention> routingConventions,
            object queryConstraints,
            object headerConstraints)
            : base(pathHandler, model, routeName, routingConventions)
        {
            QueryStringConstraints = new HttpRouteValueDictionary(queryConstraints);
            HeaderConstraints = new HttpRouteValueDictionary(headerConstraints);
        }

        public Dictionary<string, object> QueryStringConstraints { get; set; }
        public Dictionary<string, object> HeaderConstraints { get; set; }

        public override bool Match(
            HttpRequestMessage request,
            IHttpRoute route,
            string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            foreach (string key in HeaderConstraints.Keys)
            {
                if (!request.Headers.Contains(key)
                    || string.Compare(request.Headers.GetValues(key).FirstOrDefault(), HeaderConstraints[key].ToString(), true) != 0)
                {
                    return false;
                }
            }

            var queries = request.GetQueryNameValuePairs().ToDictionary(p => p.Key, p => p.Value);
            foreach (var key in QueryStringConstraints.Keys)
            {
                if (!queries.ContainsKey(key)
                    || string.Compare(queries[key], QueryStringConstraints[key].ToString(), true) != 0)
                {
                    return false;
                }
            }

            return base.Match(request, route, parameterName, values, routeDirection);
        }
    }
}