using System;
using System.Collections.Generic;

namespace ODataOpenTypeSample
{
    public class Account
    {
        public Account()
        {
            DynamicProperties = new Dictionary<string, object>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public IDictionary<string, object> DynamicProperties { get; set; }
    }

    // Address is an open complex type
    public class Address
    {
        public Address()
        {
            DynamicProperties = new Dictionary<string, object>();
        }

        public string City { get; set; }
        public string Street { get; set; }

        // If a property of the type Dictionary<string, object> is defined, then the containing type is an open type, 
        // and the key-value pairs inside this property are called  dynamic proerpties.
        public Dictionary<string, object> DynamicProperties { get; set; }
    }


}
