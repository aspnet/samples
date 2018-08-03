using System;
using System.Collections.Generic;

namespace ODataQueryableSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthTime { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
