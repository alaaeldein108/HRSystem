using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;

namespace HrSystem.PL.Models
{
	public class SignInViewModel
	{
		[Required]
		[EmailAddress(ErrorMessage = "Invalid Format For Email")]
		public string Email { get; set; }
		[Required]
		[MaxLength(6)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }


	}
}
