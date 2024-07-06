using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.ViewModel
{
    public class GeneralSettingDto
    {
        public int OvertimeHours { get; set; }

        public int DiscountHours { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public string Vacations { get; set; }
        
    }
}
