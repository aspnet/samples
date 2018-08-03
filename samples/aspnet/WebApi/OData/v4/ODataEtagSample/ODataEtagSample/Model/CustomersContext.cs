using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ODataEtagSample.Model
{
    public class CustomersContext : DbContext
    {
        static CustomersContext()
        {
            Database.SetInitializer<CustomersContext>(new CustomersContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // By default Entity Framework uses has a convention that marks Id properties in entities as
            // primary keys and gives them an Identity database generated option in the database when it
            // creates it. In this sample we want the customer to be responsible for defining the key of
            // its entities so we are overriding it.
            modelBuilder.Entity<Customer>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
        private class CustomersContextInitializer : DropCreateDatabaseAlways<CustomersContext>
        {
            protected override void Seed(CustomersContext context)
            {
                context.Customers.AddRange(Enumerable.Range(1, 10).Select(i => new Customer
                {
                    Id = i,
                    Age = 18 + i,
                    Name = "Customer " + i,
                }));
            }
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
