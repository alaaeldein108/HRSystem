using HrSystem.BLL.Interfaces;
using HrSystem.BLL.Repositories;
using HrSystem.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public IAttendanceRepository AttendanceRepository { get ; set ; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IGeneralSettingRepository GeneralSettingRepository { get; set; }
        public ISalaryReportRepository SalaryReportRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            AttendanceRepository = new AttendanceRepository(context);
            EmployeeRepository = new EmployeeRepository(context);
            GeneralSettingRepository = new GeneralSettingRepository(context);
            SalaryReportRepository = new SalaryReportRepository(context);
            VacationRepository=new VacationRepository(context);
        }
        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
