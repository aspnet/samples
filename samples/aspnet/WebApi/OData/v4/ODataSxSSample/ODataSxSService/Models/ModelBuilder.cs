using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;

namespace ODataSxSService.Models
{
    public static class ModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Order>("Orders");
            return builder.GetEdmModel();
        }
    }
}