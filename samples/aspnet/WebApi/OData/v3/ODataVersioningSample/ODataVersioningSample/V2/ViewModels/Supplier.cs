using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.V2.ViewModels
{
    public class Supplier
    {
        public Supplier()
        {
            ProductFamilies = new List<ProductFamily>();
            Address = new Address();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public virtual ICollection<ProductFamily> ProductFamilies { get; set; }
    }
}