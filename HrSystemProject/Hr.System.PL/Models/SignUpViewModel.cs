using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;
using HrSystem.DAL.ViewModel;

namespace HrSystem.PL.Models
{
    public class SignUpViewModel
    {

        public List<ApplicationRole> Roles { get; set; }
        public List<UserRolesDto> UserRoles { get; set; }

        public string Id { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Format For Email")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(6)]
        public string OldPassword { get; set; }
        [Required]
        [MaxLength(6)]
        public string Password { get; set; }
        [Required]
        [MaxLength(6)]
        [Compare(nameof(Password), ErrorMessage = "Password MisMatch")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatorId { get; set; }
        public string ModifierId { get; set; }


    }
}
