using System;
using System.Collections.Generic;

namespace ODataSxSServiceV2.Models
{
    public class Order
    {
        public int OrderId
        {
            get;
            set;
        }

        public string Store
        {
            get;
            set;
        }

        public DateTimeOffset OrderDateTime
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