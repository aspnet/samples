using System;
using System.Collections.Generic;

namespace SelectExpandDollarValueSample.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public Address BillingAddress { get; set; }
        public virtual ICollection<OrderDetail> OrderItems { get; set; }
    }
}
