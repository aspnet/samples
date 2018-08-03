using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataVersioningSample.V2.ViewModels
{
    public class ProductFamily
    {
        public ProductFamily()
        {
            Products = new List<Product>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}