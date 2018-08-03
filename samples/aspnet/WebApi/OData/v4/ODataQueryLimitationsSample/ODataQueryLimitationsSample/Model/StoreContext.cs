using System;
using System.Data.Entity;
using System.Linq;

namespace ODataQueryLimitationsSample.Model
{
    public class StoreContext : DbContext
    {
        static StoreContext()
        {
            Database.SetInitializer<StoreContext>(new StoreContextInitializer());
        }

        private class StoreContextInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
        {
            protected override void Seed(StoreContext context)
            {
                context.Customers.AddRange(Enumerable.Range(1, 10).Select(i => new Customer
                {
                    Id = i,
                    Age = 18 + i,
                    Name = "Customer " + i,
                    Address = new Address
                    {
                        FirstLine = "First line " + i,
                        SecondLine = "Second line " + i,
                        ZipCode = i * 365,
                    },
                    Orders = Enumerable.Range(1, i).Select(j => new Order
                    {
                        PurchasedOn = DateTimeOffset.Parse("1/8/2014").Subtract(TimeSpan.FromDays(i * 10 + j))
                    }).ToList()
                }));
            }
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
