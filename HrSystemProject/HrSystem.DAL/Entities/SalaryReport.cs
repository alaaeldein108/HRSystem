using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class SalaryReport : BaseEntity
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? AttendanceDays { get; set; }
        public int? AbsentDays { get; set; }
        public int? TotalOverTimeHours { get; set; }
        public int? TotalDiscountHours { get; set; }
        [Column(TypeName = ("money"))]
        public int? TotalExtra { get; set; }

        [Column(TypeName = ("money"))]
        public int? TotalDiscount { get; set; }

        [Column(TypeName = ("money"))]
        public int? TotalSalary { get; set; }

    }
}
