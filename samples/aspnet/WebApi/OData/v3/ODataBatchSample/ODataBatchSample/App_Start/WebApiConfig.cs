using ODataBatchSample.Models;
using System.Web.Http;
using System.Web.Http.OData.Batch;
using System.Web.Http.OData.Builder;

using Microsoft.Data.Edm;

namespace ODataBatchSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapODataRoute("odata", "odata", GetModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
        }

        private static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.ContainerName = "CustomersContext";
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }
}
