using System.Collections.Generic;

namespace ODataAttributeRoutingSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Order> Orders { get; set; }
    }
}