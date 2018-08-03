using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataSxSServiceV2.Models
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