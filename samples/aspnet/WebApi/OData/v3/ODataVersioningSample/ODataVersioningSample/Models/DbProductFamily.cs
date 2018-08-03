using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.Models
{
    // V2 new entity
    public class DbProductFamily
    {
        public DbProductFamily()
        {
            Products = new List<DbProduct>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<DbProduct> Products { get; set; }

        public virtual DbSupplier Supplier { get; set; }
    }
}