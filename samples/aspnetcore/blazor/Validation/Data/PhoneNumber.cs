using System.ComponentModel.DataAnnotations;

namespace Validation.Data
{
    public class PhoneNumber
    {
        public string Description { get; set; }

        [Required]
        public string Number { get; set; }
    }
}
