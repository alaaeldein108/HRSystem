using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;

namespace HrSystem.PL.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


    }
}
