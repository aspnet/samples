using ODataBatchSample.Models;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

using Microsoft.OData.Edm;

namespace ODataBatchSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("odata", "odata", GetModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
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
