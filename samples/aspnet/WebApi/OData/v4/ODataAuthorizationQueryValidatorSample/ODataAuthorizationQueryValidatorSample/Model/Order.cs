using System.Collections.Generic;

namespace ODataAuthorizationQueryValidatorSample.Model
{
    public class Order
    {
        public int Id { get; set; }
        public ICollection<OrderLine> Items { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
