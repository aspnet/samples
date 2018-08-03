
using System;
using System.Web;
using System.Web.Routing;

namespace AttributeRoutingSample
{
    internal class VersionedRouteConstraint : IRouteConstraint
    {
        public VersionedRouteConstraint(int version)
        {
            Version = version;
        }

        public int Version
        {
            get;
            private set;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object obj;
            if (!values.TryGetValue("version", out obj))
            {
                // No version was specifed, assume Version == 1.
                return Version == 1;
            }

            int? version = obj as int?;
            if (version == Version)
            {
                return true;
            }

            int parsedVersion;
            if (!Int32.TryParse(obj as string, out parsedVersion))
            {
                // Invalid version parameter, assume no match.
                return false;
            }

            return parsedVersion == Version;
        }
    }
}
