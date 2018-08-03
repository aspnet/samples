using System.Collections.Generic;
using System.Web.OData.Query;

namespace ODataQueryLimitationsSample.Model
{
    public class Customer
    {
        public int Id { get; set; }

        [NonFilterable]
        [Unsortable]
        public string Name { get; set; }
        public int Age { get; set; }

        [NonFilterable]
        public Address Address { get; set; }

        [NotExpandable]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
