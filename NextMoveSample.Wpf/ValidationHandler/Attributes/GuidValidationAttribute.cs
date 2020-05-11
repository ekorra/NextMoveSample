using System;
using System.ComponentModel.DataAnnotations;

namespace NextMoveSample.Wpf.ValidationHandler.Attributes
{
    public class GuidValidationAttribute: ValidationAttribute
    {
       
        public string GetErrorMessage() => "Must be valid UUID";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!Guid.TryParse((string)value, out _))
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
