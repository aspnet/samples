using System.Data.Entity;
using System.IO;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;

namespace ODataEFBatchSample.Models
{
    public class ShoppingContext : DbContext
    {
        static ShoppingContext()
        {
            Database.SetInitializer<ShoppingContext>(new DropCreateDatabaseAlways<ShoppingContext>());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrdersLines { get; set; }

        /// <summary>
        /// Reads the <see cref="IEdmModel"/> associated with the <see cref="ShoppingContext"/>.
        /// </summary>
        /// <returns>The <see cref="IEdmModel"/> associated with the <see cref="ShoppingContext"/>.</returns>
        public IEdmModel GetEdmModel()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    System.Data.Entity.Infrastructure.EdmxWriter.WriteEdmx(this, writer);
                }

                stream.Position = 0;

                using (XmlReader reader = XmlReader.Create(stream))
                {
                    IEdmModel model = EdmxReader.Parse(reader);
                    return model;
                }
            }
        }
    }
}