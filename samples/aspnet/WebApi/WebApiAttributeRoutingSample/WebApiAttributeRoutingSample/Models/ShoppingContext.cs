using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiAttributeRoutingSample.Models
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext() : base("name=ShoppingContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order_Detail> Order_Details { get; set; }
    }
}
