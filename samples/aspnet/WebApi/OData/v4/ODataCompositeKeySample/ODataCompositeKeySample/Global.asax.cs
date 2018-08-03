using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using ODataCompositeKeySample.Models;
using ODataCompositeKeySample.Extensions;

namespace ODataCompositeKeySample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            var mb = new ODataConventionModelBuilder(config);
            mb.EntitySet<Person>("People");

            // Add the CompositeKeyRoutingConvention.
            var conventions = ODataRoutingConventions.CreateDefault();
            conventions.Insert(0, new CompositeKeyRoutingConvention());

            config.MapODataServiceRoute(
                routeName: "OData",
                routePrefix: null,
                model: mb.GetEdmModel(),
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions);
        }
    }
}