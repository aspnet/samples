using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ValidationSample.Validators
{
    /// <summary>
    /// This is a custom validation attribute that will raise validation errors if an int is negative.
    /// </summary>
    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is int))
            {
                return new ValidationResult("NonNegativeAttribute can only be applied to ints.");
            }
            if ((int)value < 0)
            {
                return new ValidationResult("Property should be non-negative.");
            }
            return null;
        }
    }
}
