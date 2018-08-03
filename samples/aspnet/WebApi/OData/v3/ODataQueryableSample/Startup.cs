using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm.Library;
using ODataQueryableSample.Models;
using Owin;
using Microsoft.Data.Edm;

namespace ODataQueryableSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Set up server configuration
            var config = new HttpConfiguration();
            config.Routes.MapODataRoute(routeName: "OData", routePrefix: "odata", model: GetEdmModel());
            appBuilder.UseWebApi(config);
        }

        private IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Customer>("customer");
            modelBuilder.EntitySet<Order>("order");
            modelBuilder.EntitySet<Customer>("response");
            return modelBuilder.GetEdmModel();
        }
    }
}
