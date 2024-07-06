using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class GeneralSetting : BaseEntity
    {
        public int Id { get; set; }
        public int? OvertimeHours { get; set; }
        public int? DiscountHours { get; set; }
        public string WeeklyDay { get; set; }
        /*public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public Vacation Vacation { get; set; }
        public int VacationId { get; set; }*/
    }
}
