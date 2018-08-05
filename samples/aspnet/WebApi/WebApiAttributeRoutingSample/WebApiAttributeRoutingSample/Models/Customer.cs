using System;
using System.Collections.Generic;

namespace WebApiAttributeRoutingSample.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
