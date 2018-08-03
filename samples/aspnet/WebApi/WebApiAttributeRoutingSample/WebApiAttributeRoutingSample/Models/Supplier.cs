using System;
using System.Collections.Generic;

namespace WebApiAttributeRoutingSample.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.Products = new List<Product>();
        }

        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
