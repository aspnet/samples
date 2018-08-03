using System;
using System.Collections.Generic;

namespace WebApiAttributeRoutingSample.Models
{
    public class Product
    {
        public Product()
        {
            this.Order_Details = new List<Order_Detail>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
