using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.Models
{
    // V2 new entity
    public class DbSupplier
    {
        public DbSupplier()
        {
            ProductFamilies = new List<DbProductFamily>();
            Address = new DbAddress();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public DbAddress Address { get; set; }

        public virtual ICollection<DbProductFamily> ProductFamilies { get; set; }
    }
}