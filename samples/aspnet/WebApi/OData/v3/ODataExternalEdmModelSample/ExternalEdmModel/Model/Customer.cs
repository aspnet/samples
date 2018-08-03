using System.Collections.Generic;

namespace ExternalEdmModel.Model
{
    public class Customer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
