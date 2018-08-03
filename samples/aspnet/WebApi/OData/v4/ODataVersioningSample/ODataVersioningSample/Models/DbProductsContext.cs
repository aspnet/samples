using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ODataVersioningSample.Models
{
    public class DbProductsContext : DbContext
    {
        public DbProductsContext()
            : base("DbProducts")
        {
            Database.SetInitializer(new DbProductsContextInitializer());
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<DbProduct> Products { get; set; }
        public DbSet<DbSupplier> Suppliers { get; set; }
        public DbSet<DbProductFamily> ProductFamilies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbProduct>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<DbProductFamily>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<DbSupplier>().Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            base.OnModelCreating(modelBuilder);
        }
    }
}