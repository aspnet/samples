using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ODataModelAliasingSample.Model
{
    public class CustomersContext : DbContext
    {
        static CustomersContext()
        {
            Database.SetInitializer<CustomersContext>(new CustomersContextInitializer());
        }

        private class CustomersContextInitializer : DropCreateDatabaseIfModelChanges<CustomersContext>
        {
            protected override void Seed(CustomersContext context)
            {
                IList<CustomerDto> customers = Enumerable.Range(1, 10).Select(i => new CustomerDto
                {
                    Id = i,
                    GivenName = "First name " + i,
                    LastName = "Last name " + i,
                    Purchases = Enumerable.Range(1, i).Select(j => new OrderDto
                    {
                        Id = i * 10 + j,
                        Total = (i * 10 + j) * 3
                    }).ToList()
                }).ToList();

                context.Customers.AddRange(customers);

                base.Seed(context);
            }
        }

        public DbSet<CustomerDto> Customers { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
    }
}
