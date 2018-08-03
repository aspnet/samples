using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ODataService.Models
{
    public class ProductsContext : DbContext
    {
        static ProductsContext()
        {
            Database.SetInitializer(new ProductsContextInitializer());
        }
        
        public ProductsContext()
            : base("Products")
        {       
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ProductFamily>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Supplier>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductFamily> ProductFamilies { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
