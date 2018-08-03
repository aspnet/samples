using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;
using ODataPagingSample.Models;

namespace ODataPagingSample
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Creates the model for our Movies entity set
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Movie>("Movies");
            IEdmModel model = modelBuilder.GetEdmModel();

            // Adds an OData route with the 'api' prefix. Requests can then be made to /api/Movies for example
            GlobalConfiguration.Configuration.Routes.MapODataRoute(routeName: "OData", routePrefix: "api", model: model);
        }
    }
}