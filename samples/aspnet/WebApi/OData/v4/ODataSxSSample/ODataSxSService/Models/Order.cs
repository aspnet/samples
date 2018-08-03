using System;
using System.Collections.Generic;

namespace ODataSxSService.Models
{
    public class Order
    {
        public int OrderId
        {
            get;
            set;
        }

        public DateTime OrderDateTime
        {
            get;
            set;
        }

        public virtual ICollection<Product> Products
        {
            get;
            set;
        }
    }
}