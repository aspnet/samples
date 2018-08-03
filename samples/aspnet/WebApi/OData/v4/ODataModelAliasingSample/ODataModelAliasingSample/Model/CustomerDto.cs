using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ODataModelAliasingSample.Model
{
    [DataContract(Name = "Customer")]
    public class CustomerDto
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember(Name = "FirstName")]
        public string GivenName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember(Name = "Orders")]
        public virtual ICollection<OrderDto> Purchases { get; set; }
    }
}
