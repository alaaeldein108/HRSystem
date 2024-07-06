using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hr.System.PL.Models.ValidationAttributes;

namespace HrSystem.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
		[MinAge(18, ErrorMessage = "Employee must be at least 18 years old.")]
		public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public decimal Salary { get; set; }
		[ExactLength(14)]
		public long NationalId { get; set; }
        public string Nationality { get; set; }
        public TimeSpan CheckInTime { get; set; } = new TimeSpan(9, 0, 0);
        [CheckOutTimeAfterCheckInTime]
        public TimeSpan CheckOutTime { get; set; }= new TimeSpan(17, 0, 0);
        public bool IsActive { get; set; } = true;
		public int DepartmentId { get; set; }
		public IFormFile? Image { get; set; }
		public string? ImageUrl { get; set; }
		public string? Notes { get; set; }
        public decimal? HourRate { get; set; }
       
    }
}
