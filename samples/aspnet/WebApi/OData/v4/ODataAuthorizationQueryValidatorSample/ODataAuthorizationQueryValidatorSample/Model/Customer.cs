using System.Collections.Generic;

namespace ODataAuthorizationQueryValidatorSample.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
