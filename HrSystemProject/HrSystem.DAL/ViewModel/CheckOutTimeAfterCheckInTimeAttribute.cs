using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.ViewModel
{
    public class CheckOutTimeAfterCheckInTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var attendance = (Attendance)validationContext.ObjectInstance;
            if (attendance.CheckOutTime == null)
            {
                return ValidationResult.Success;
            }
            if (attendance.CheckOutTime < attendance.CheckInTime)
            {
                return new ValidationResult("Check-out time must be after check-in time.");
            }

            return ValidationResult.Success;
        }
    }
}
