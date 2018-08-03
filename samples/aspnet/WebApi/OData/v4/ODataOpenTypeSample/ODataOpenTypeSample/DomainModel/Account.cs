using System.Collections.Generic;

namespace ODataOpenTypeSample
{
    // Account is an open entity type
    public class Account
    {
        public Account()
        {
            DynamicProperties = new Dictionary<string, object>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        // If a property of the type Dictionary<string, object> is defined, then the containing type is an open type, 
        // and the key-value pairs inside this property are called dynamic proerpties.
        public IDictionary<string, object> DynamicProperties { get; set; }
    }
}
