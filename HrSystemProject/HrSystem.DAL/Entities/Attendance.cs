using HrSystem.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class Attendance : BaseEntity
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        [Column(TypeName = ("date"))]
        public DateTime Date { get; set; }= DateTime.Now;
        public TimeSpan? CheckInTime { get; set; }
        [CheckOutTimeAfterCheckInTime]
        public TimeSpan? CheckOutTime { get; set; }
       

    }
}
