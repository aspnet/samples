using System.Data.Entity;
using System.Linq;

namespace ODataDollarFormatSample.Model
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
                context.Customers.AddRange(Enumerable.Range(1, 10).Select(i => new Customer
                {
                    Id = i,
                    Name = "Name " + i
                }));
            }
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
