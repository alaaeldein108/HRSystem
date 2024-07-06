using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Column(TypeName = ("date"))]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        [Column(TypeName = ("date"))]
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public int? WorkHours { get; set; }

        [Column(TypeName = ("money"))]
        public decimal Salary { get; set; }
        public decimal? HourRate { get; set; }

        public long NationalId { get; set; }
        public string Nationality { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<SalaryReport> SalaryReports { get; set; } = new List<SalaryReport>();

        public ICollection<GeneralSettingsNew> GeneralSettings { get; set; } = new List<GeneralSettingsNew>();


    }
}
