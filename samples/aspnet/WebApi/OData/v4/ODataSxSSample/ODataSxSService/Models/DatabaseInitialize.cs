using System;
using System.Data.Entity;
using System.Linq;

namespace ODataSxSService.Models
{
    public class DatabaseInitialize : DropCreateDatabaseAlways<ODataSxSServiceContext>
    {
        protected override void Seed(ODataSxSServiceContext context)
        {
            context.Orders.AddRange(Enumerable.Range(1, 5).Select(n =>
            {
                var order = new Order() { OrderDateTime = DateTime.Now };

                order.Products = Enumerable.Range(0, 2)
                    .Select(p => new Product() { Title = string.Concat("Old Product_", n), ManufactureDateTime = DateTime.Now }).ToList();

                return order;
            }));
        }
    }
}