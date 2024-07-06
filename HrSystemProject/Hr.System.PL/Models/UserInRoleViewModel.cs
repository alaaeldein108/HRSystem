using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;
using HrSystem.DAL.ViewModel;

namespace HrSystem.PL.Models
{
    public class UserInRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSelected { get; set; }

    }
}
