using HrSystem.PL.Models;
using System.ComponentModel.DataAnnotations;

namespace Hr.System.PL.Models.ValidationAttributes
{
    public class CheckOutTimeAfterCheckInTimeAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (EmployeeViewModel)validationContext.ObjectInstance;
            if(employee.CheckOutTime == null)
            {
                return ValidationResult.Success;
            }
            if (employee.CheckOutTime < employee.CheckInTime)
            {
                return new ValidationResult("Check-out time must be after check-in time.");
            }

            return ValidationResult.Success;
        }
    }
}
