using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BatchSample.Models
{
    public class CustomersContextInitializer : DropCreateDatabaseAlways<CustomersContext>
    {
        protected override void Seed(CustomersContext context)
        {
            IList<Customer> customers = Enumerable.Range(0, 10).Select(i =>
                new Customer
                {
                    Id = i,
                    Name = "Name " + i
                }).ToList();
            context.Customers.AddRange(customers);
        }
    }
}