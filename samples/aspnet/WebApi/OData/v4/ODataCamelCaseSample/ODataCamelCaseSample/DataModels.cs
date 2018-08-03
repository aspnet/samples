using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ODataCamelCaseSample
{
    [DataContract]
    public class Employee
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember(Name = "Name")]
        public string FullName { get; set; }

        [DataMember]
        public Gender Sex { get; set; }

        [DataMember]
        public Address Address { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }
}
