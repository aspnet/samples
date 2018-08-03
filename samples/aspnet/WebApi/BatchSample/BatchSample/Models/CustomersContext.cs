using System.Data.Entity;

namespace BatchSample.Models
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}