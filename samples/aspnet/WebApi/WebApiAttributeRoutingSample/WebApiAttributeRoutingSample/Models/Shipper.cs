using System;
using System.Collections.Generic;

namespace WebApiAttributeRoutingSample.Models
{
    public class Shipper
    {
        public Shipper()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
