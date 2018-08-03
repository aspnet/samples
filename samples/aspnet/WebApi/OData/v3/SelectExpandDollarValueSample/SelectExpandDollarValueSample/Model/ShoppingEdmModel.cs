using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;

namespace SelectExpandDollarValueSample.Model
{
    public static class ShoppingEdmModel
    {
        public static IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Customer> customers = builder.EntitySet<Customer>("Customers");
            EntitySetConfiguration<Order> orders = builder.EntitySet<Order>("Orders");
            EntitySetConfiguration<OrderDetail> orderItems = builder.EntitySet<OrderDetail>("OrderItems");

            return builder.GetEdmModel();
        }
    }
}
