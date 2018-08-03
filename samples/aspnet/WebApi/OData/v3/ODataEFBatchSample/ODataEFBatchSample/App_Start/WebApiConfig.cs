using System.Web.Http;
using System.Web.Http.OData.Batch;
using System.Web.Http.OData.Extensions;
using Microsoft.Data.Edm;
using ODataEFBatchSample.Extensions;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Read the model directly from entity framework.
            IEdmModel model;
            using (ShoppingContext context = new ShoppingContext())
            {
                model = context.GetEdmModel();
            }

            // Create our batch handler and associate it with the OData service.
            ODataBatchHandler batchHandler = new EntityFrameworkBatchHandler(GlobalConfiguration.DefaultServer);
            config.Routes.MapODataServiceRoute("odata", "odata", model, batchHandler);
        }
    }
}
