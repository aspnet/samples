using System.Data.Entity;

namespace ODataBatchSample.Models
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}