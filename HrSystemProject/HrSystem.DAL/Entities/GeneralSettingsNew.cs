using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class GeneralSettingsNew
    {
        [Required]
        public int OvertimeHours { get; set; }
        [Required]
        public int DiscountHours { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public Vacation Vacation { get; set; }
        public int VacationId { get; set; }
    }
}
