using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ValidationSample.Validators;

namespace ValidationSample.Models
{
    [DataContract]
    public class Customer
    {
        [NonNegative]
        [DataMember(IsRequired=true)]
        public int ID
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [RegularExpression(@"^(\((\d{3})\)|(\d{3}))(\s|-)?(\d{3})-?(\d{4})$")]
        [DataMember]
        public string PhoneNumber
        {
            get;
            set;
        }
    }
}
