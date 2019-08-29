using System;
using System.ComponentModel.DataAnnotations;

namespace Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ValidateRecursiveAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            RecursiveDataAnnotationsValidator.TryValidateRecursive(value, validationContext);
            return ValidationResult.Success;
        }
    }
}
