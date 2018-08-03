using System.Data.Entity;

namespace ODataSxSServiceV2.Models
{
    public class ODataSxSServiceContext : DbContext
    {
        public ODataSxSServiceContext() : base("name=ODataSxSServiceV2Context")
        {
        }

        public System.Data.Entity.DbSet<Product> Products { get; set; }
        public System.Data.Entity.DbSet<Order> Orders { get; set; }
    }
}
