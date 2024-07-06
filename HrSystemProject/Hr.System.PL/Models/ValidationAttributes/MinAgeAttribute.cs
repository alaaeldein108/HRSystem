using System.ComponentModel.DataAnnotations;

namespace Hr.System.PL.Models.ValidationAttributes
{
	public class MinAgeAttribute: ValidationAttribute
	{
		private readonly int _minAge;

		public MinAgeAttribute(int minAge)
		{
			_minAge = minAge;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is DateTime dateOfBirth)
			{
				var age = DateTime.Today.Year - dateOfBirth.Year;
				if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

				if (age >= _minAge)
				{
					return ValidationResult.Success;
				}
				else
				{
					return new ValidationResult($"Age must be at least {_minAge} years.");
				}
			}
			return new ValidationResult("Invalid birth date");
		}
	}
}
