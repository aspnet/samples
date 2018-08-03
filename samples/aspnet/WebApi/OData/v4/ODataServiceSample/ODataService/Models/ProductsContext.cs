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
            //// Remove the following line after fixing https://aspnetwebstack.codeplex.com/workitem/1768
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ProductFamily>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Supplier>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFamily> ProductFamilies { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
