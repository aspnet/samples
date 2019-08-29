using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Validation.Data
{
    public class Person
    {
        [Required]
        public string Name { get; set; }

        [ValidateRecursive]
        public Address Address { get; set; } = new Address();

        [ValidateRecursive]
        public PhoneNumber[] PhoneNumbers { get; set; } = new[]
        {
            new PhoneNumber(),
            new PhoneNumber(),
        };
    }
}
