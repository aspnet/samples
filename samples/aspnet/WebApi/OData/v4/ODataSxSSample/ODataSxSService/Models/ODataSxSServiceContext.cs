using System.Data.Entity;

namespace ODataSxSService.Models
{
    public class ODataSxSServiceContext : DbContext
    {
        public ODataSxSServiceContext() : base("name=ODataSxSServiceContext")
        {
        }

        public System.Data.Entity.DbSet<Product> Products { get; set; }
        public System.Data.Entity.DbSet<Order> Orders { get; set; }
    }
}
