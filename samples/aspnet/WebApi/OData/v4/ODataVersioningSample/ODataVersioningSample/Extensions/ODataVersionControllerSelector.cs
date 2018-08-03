using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData.Extensions;

namespace ODataVersioningSample.Extensions
{
    /// <summary>
    /// Controllerselector that figures out the version suffix from the route name.
    /// For example: request from route V1 can be dispatched to ProductsV1Controller.
    /// </summary>
    public class ODataVersionControllerSelector : DefaultHttpControllerSelector
    {
        private Dictionary<string, string> _routeVersionSuffixMapping = new Dictionary<string, string>();

        public ODataVersionControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public Dictionary<string, string> RouteVersionSuffixMapping
        {
            get
            {
                return _routeVersionSuffixMapping;
            }
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            var controllerName = base.GetControllerName(request);
            if (string.IsNullOrEmpty(controllerName))
            {
                return controllerName;
            }

            var routeName = request.ODataProperties().RouteName;
            if (string.IsNullOrEmpty(routeName))
            {
                return controllerName;
            }

            var mapping = GetControllerMapping();

            if (!_routeVersionSuffixMapping.ContainsKey(routeName))
            {
                return controllerName;
            }

            var versionControllerName = controllerName + _routeVersionSuffixMapping[routeName];
            return mapping.ContainsKey(versionControllerName)
                ? versionControllerName
                : controllerName;
        }
    }
}