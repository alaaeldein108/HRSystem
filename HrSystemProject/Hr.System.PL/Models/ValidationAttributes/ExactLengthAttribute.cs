using System.ComponentModel.DataAnnotations;

namespace Hr.System.PL.Models.ValidationAttributes
{
    public class ExactLengthAttribute: ValidationAttribute
    {
        private readonly int _exactLength;

        public ExactLengthAttribute(int exactLength)
        {
            _exactLength = exactLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var stringValue = value.ToString();
                if (stringValue.Length != _exactLength)
                {
                    return new ValidationResult($"The field {validationContext.DisplayName} must be exactly {_exactLength} digits.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
