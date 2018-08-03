using System;
using System.Collections.Generic;

namespace WebApiAttributeRoutingSample.Models
{
    public class Order
    {
        public Order()
        {
            this.Order_Details = new List<Order_Detail>();
        }

        public int Id { get; set; }

        public int CustomerID { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string ShipName { get; set; }

        public string ShipCity { get; set; }

        public string ShipState { get; set; }

        public string ShipCountry { get; set; }

        public string ShipPostalCode { get; set; }

        public bool? IsApproved { get; set; }

        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual Shipper Shipper { get; set; }
    }
}
