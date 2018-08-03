
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace ODataSxSServiceV2.Extensions
{
    public class VersionedRoutingConvention : IODataRoutingConvention
    {
        private readonly IODataRoutingConvention _innerRoutingConvention;
        private readonly string _versionSuffix;

        public VersionedRoutingConvention(IODataRoutingConvention innerRoutingConvention, string versionSuffix)
        {
            _innerRoutingConvention = innerRoutingConvention;
            _versionSuffix = versionSuffix;
        }

        public string SelectAction(ODataPath odataPath,
            HttpControllerContext controllerContext,
            ILookup<string, HttpActionDescriptor> actionMap)
        {
            return _innerRoutingConvention.SelectAction(odataPath, controllerContext, actionMap);
        }

        /// <summary>
        /// Returns the controller names with the version suffix. 
        /// For example: request from route V1 can be dispatched to ProductsV1Controller.
        /// </summary>
        public string SelectController(ODataPath odataPath, HttpRequestMessage request)
        {
            var baseControllerName = _innerRoutingConvention.SelectController(odataPath, request);
            if (baseControllerName != null)
            {
                return string.Concat(baseControllerName, _versionSuffix);
            }

            return null;
        }
    }
}
