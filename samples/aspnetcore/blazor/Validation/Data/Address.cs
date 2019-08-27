using System;
using System.ComponentModel.DataAnnotations;

namespace Validation.Data
{
    public class Address
    {
        public string Street { get; set; }

        [Required]
        public string City { get; set; }
    }
}
