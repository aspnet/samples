using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ProductODataService.Models
{
    public class ProductsServiceEdmModel
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");

            var entityTypeConfigurationOfEmployee = builder.EntityType<Product>();

            // Function bound to a collection of base EntityType.
            entityTypeConfigurationOfEmployee.Collection.Function("GetCount").Returns<int>();

            // Overload
            entityTypeConfigurationOfEmployee.Collection.Function("GetCount").Returns<int>()
                .Parameter<string>("Name");

            builder.Namespace = typeof(Product).Namespace;
            return builder.GetEdmModel();
        }
    }
}
