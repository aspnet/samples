using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

namespace ODataCompositeKeySample.Extensions
{
    // This is a sample implementation of routing convention to support composit keys.
    // The implementation will fail if key value has ',' in it. Please implement your own 
    // convention to handle it.
    public class CompositeKeyRoutingConvention : EntityRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            var action = base.SelectAction(odataPath, controllerContext, actionMap);
            if (action != null)
            {
                var routeValues = controllerContext.RouteData.Values;
                if (routeValues.ContainsKey(ODataRouteConstants.Key))
                {
                    var keyRaw = routeValues[ODataRouteConstants.Key] as string;
                    IEnumerable<string> compoundKeyPairs = keyRaw.Split(',');
                    if (compoundKeyPairs == null || !compoundKeyPairs.Any())
                    {
                        return action;
                    }

                    foreach (var compoundKeyPair in compoundKeyPairs)
                    {
                        string[] pair = compoundKeyPair.Split('=');
                        if (pair == null || pair.Length != 2)
                        {
                            continue;
                        }
                        var keyName = pair[0].Trim();
                        var keyValue = pair[1].Trim();

                        routeValues.Add(keyName, keyValue);
                    }
                }
            }

            return action;
        }
    }
}