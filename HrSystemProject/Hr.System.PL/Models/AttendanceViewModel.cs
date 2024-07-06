using HrSystem.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.PL.Models
{
    public class AttendanceViewModel
    {
        public Attendance Attendance { get; set; }
        public List<Attendance> ListOfAttendance { get; set; }
        
    }
}
