using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IAttendanceRepository AttendanceRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IGeneralSettingRepository GeneralSettingRepository { get; set; }
        public ISalaryReportRepository SalaryReportRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }
        public int Complete();

    }
}
