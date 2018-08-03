using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace DynamicEdmModelCreation
{
    public class MatchAllRoutingConvention : IODataRoutingConvention
    {
        public string SelectAction(
            ODataPath odataPath,
            HttpControllerContext controllerContext,
            ILookup<string, HttpActionDescriptor> actionMap)
        {
            return null;
        }

        public string SelectController(ODataPath odataPath, HttpRequestMessage request)
        {
            return (odataPath.Segments.FirstOrDefault() is EntitySetPathSegment) ? "HandleAll" : null;
        }
    }
}