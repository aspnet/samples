using System.Collections.Generic;

namespace SelectExpandDollarValueSample.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
