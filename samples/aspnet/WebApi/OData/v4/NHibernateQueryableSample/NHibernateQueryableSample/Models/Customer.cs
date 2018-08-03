using System.Collections.Generic;

namespace NHibernateQueryableSample.Models
{
    public class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
        }

        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual IList<Order> Orders { get; set; }
    }
}
