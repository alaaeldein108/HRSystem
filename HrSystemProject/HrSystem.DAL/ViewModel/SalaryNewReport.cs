using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.ViewModel
{
    public class SalaryNewReport
    {
        public Employee Employee { get; set; }
        public int AttendanceDays { get; set; }
        public int AbsentDays { get; set; }
        public decimal TotalOverTimeHours { get; set; }
        public decimal TotalDiscountHours { get; set; }
        public decimal TotalExtra { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalSalary { get; set; }
    }

}
