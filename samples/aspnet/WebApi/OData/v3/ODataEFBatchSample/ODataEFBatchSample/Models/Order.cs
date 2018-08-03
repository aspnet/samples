using System.Collections.Generic;

namespace ODataEFBatchSample.Models
{
    public class Order
    {
        private ICollection<OrderLine> _orderLines = new List<OrderLine>();

        public virtual int Id { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual ICollection<OrderLine> OrderLines
        {
            get { return _orderLines; }
            set { _orderLines = value; }
        }
    }
}
