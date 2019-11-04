using System.ComponentModel.DataAnnotations;

namespace Validation.Data
{
    public class Person
    {
        [Required]
        public string Name { get; set; }

        [ValidateComplexType]
        public Address Address { get; set; } = new Address();

        [ValidateComplexType]
        public PhoneNumber[] PhoneNumbers { get; set; } = new[]
        {
            new PhoneNumber(),
            new PhoneNumber(),
        };
    }
}
