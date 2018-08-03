using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace RouteConstraintsAndModelBindersSample.Server.RouteConstraints
{
    /// <summary>
    /// This constraint will match a route starting with the segmentPrefix + ";" or the segmentPrefix only.
    /// </summary>
    /// <example>
    /// If Route["{apples:SegmentPrefix}"] is specified, .../apples;color=red/... or .../apples will
    /// match, but .../apples?color=red will not.
    /// </example>
    public class SegmentPrefixConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request,
                          IHttpRoute route,
                          string segmentPrefix,
                          IDictionary<string, object> values,
                          HttpRouteDirection routeDirection)
        {
            if (segmentPrefix == null)
            {
                throw new ArgumentNullException("segmentPrefix");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            object value;
            if (values.TryGetValue(segmentPrefix, out value))
            {
                string valueString = value as string;
                return valueString != null
                       && (valueString.StartsWith(segmentPrefix + ";", StringComparison.OrdinalIgnoreCase)
                           || String.Equals(valueString, segmentPrefix, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }
    }
}
