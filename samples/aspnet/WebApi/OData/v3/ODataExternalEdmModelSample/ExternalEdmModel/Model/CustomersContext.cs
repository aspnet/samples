using System.Data.Entity;
using System.IO;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;

namespace ExternalEdmModel.Model
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

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
                    return EdmxReader.Parse(reader);
                }
            }
        }
    }
}
