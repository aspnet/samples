
using System;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace AttributeRoutingSample
{
    /// <summary>
    /// A route attribute that defines a version property. This will create a route entry with a constraint
    /// that matches the 'version' parameter from the route template.
    /// </summary>
    internal class VersionedRouteAttribute : RouteFactoryAttribute
    {
        public VersionedRouteAttribute(string template, int version)
            : base(template)
        {
            Version = Math.Max(1, version);
        }

        public int Version
        {
            get;
            private set;
        }

        public override RouteValueDictionary Constraints
        {
            get
            {
                var constraints = new RouteValueDictionary();
                constraints.Add("version", new VersionedRouteConstraint(Version));
                return constraints;
            }
        }

        public override RouteValueDictionary Defaults
        {
            get
            {
                var defaults = new RouteValueDictionary();
                defaults.Add("version", Version);
                return defaults;
            }
        }
    }
}