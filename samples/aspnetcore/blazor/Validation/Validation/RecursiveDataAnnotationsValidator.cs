using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Validation
{
    public class RecursiveDataAnnotationsValidator : ComponentBase
    {
        private static readonly object validationContextValidatorKey = new object();
        private ValidationMessageStore messages;

        [CascadingParameter] EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            messages = new ValidationMessageStore(EditContext);

            // Perform object-level validation (starting from the root model) on request
            EditContext.OnValidationRequested += (sender, eventArgs) =>
            {
                messages.Clear();
                ValidateObject(EditContext.Model);
                EditContext.NotifyValidationStateChanged();
            };

            // Perform per-field validation on each field edit
            EditContext.OnFieldChanged += (sender, eventArgs) =>
                ValidateField(EditContext, messages, eventArgs.FieldIdentifier);
        }

        private void ValidateObject(object value)
        {
            if (value is IEnumerable<object> enumerable)
            {
                foreach (var item in enumerable)
                {
                    ValidateObject(item);
                }

                return;
            }

            var validationResults = new List<ValidationResult>();
            ValidateObject(value, validationResults);

            // Transfer results to the ValidationMessageStore
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    var fieldIdentifier = new FieldIdentifier(value, memberName);
                    messages.Add(fieldIdentifier, validationResult.ErrorMessage);
                }
            }
        }

        private void ValidateObject(object value, List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(value);
            validationContext.Items.Add(validationContextValidatorKey, this);
            Validator.TryValidateObject(value, validationContext, validationResults, validateAllProperties: true);
        }

        internal static void TryValidateRecursive(object value, ValidationContext validationContext)
        {
            if (validationContext.Items.TryGetValue(validationContextValidatorKey, out var validator))
            {
                ((RecursiveDataAnnotationsValidator)validator).ValidateObject(value);
            }
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            // DataAnnotations only validates public properties, so that's all we'll look for
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(fieldIdentifier.Model);
                var validationContext = new ValidationContext(fieldIdentifier.Model)
                {
                    MemberName = propertyInfo.Name
                };
                var results = new List<ValidationResult>();

                Validator.TryValidateProperty(propertyValue, validationContext, results);
                messages.Clear(fieldIdentifier);
                messages.Add(fieldIdentifier, results.Select(result => result.ErrorMessage));

                // We have to notify even if there were no messages before and are still no messages now,
                // because the "state" that changed might be the completion of some async validation task
                editContext.NotifyValidationStateChanged();
            }
        }
    }
}
