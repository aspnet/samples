using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ExternalEdmModel.Model
{
    public class CustomersContextInitializer : DropCreateDatabaseAlways<CustomersContext>
    {
        protected override void Seed(CustomersContext context)
        {
            IList<Customer> customers = Enumerable.Range(0, 10).Select(i => new Customer
            {
                Id = i,
                Name = "Name " + i,
                Orders = Enumerable.Range(0, i).Select(j => new Order
                {
                    CustomerId = i,
                    Id = j,
                    Total = j * 10
                }).ToList()
            }).ToList();

            foreach (Customer customer in customers)
            {
                context.Customers.Add(customer);
            }

            base.Seed(context);
        }
    }
}
