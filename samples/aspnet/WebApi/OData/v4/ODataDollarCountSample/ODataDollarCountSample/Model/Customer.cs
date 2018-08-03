using System.Collections.Generic;
using System.Web.OData.Query;

namespace ODataDollarCountSample.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> Emails { get; set; }
        public IList<Address> ShipAddresses { get; set; }
        [NotCountable]
        public IList<Day> AvailableDays { get; set; }
    }
}
