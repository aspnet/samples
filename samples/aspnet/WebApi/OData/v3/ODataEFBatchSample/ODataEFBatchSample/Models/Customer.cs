using System.Collections.Generic;

namespace ODataEFBatchSample.Models
{
    public class Customer
    {
        private ICollection<Order> _orders = new List<Order>();

        public virtual int Id { get; set; }
        public virtual ICollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }
    }
}