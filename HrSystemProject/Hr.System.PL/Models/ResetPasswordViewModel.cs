using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;

namespace HrSystem.PL.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [Compare(nameof(Password), ErrorMessage = "Password Mismatoch")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
