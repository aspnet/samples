using System;
using System.Data.Entity;
using System.Linq;

namespace ODataSxSServiceV2.Models
{
    public class DatabaseInitialize : DropCreateDatabaseAlways<ODataSxSServiceContext>
    {
        protected override void Seed(ODataSxSServiceContext context)
        {
            context.Orders.AddRange(Enumerable.Range(1, 5).Select(n =>
            {
                var order = new Order() { OrderDateTime = DateTimeOffset.Now, Store = string.Concat("Store_", n) };

                order.Products = Enumerable.Range(0, 2)
                    .Select(p => new Product() { Title = string.Concat("Product_", n), ManufactureDateTime = DateTimeOffset.Now }).ToList();
                return order;
            }));
        }
    }
}