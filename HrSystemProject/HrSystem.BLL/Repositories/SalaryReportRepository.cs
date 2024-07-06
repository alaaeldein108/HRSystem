using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Context;
using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRSystem.BLL.Repositories
{
    class OverTimeClass
    {
        public string VacationDay { get; set; }
        public int OverTime { get; set; }

        public OverTimeClass(string VacationDay, int OverTime)
        {
            this.VacationDay = VacationDay;
            this.OverTime = OverTime;
        }
    }
    class DiscountTimeClass
    {
        public string VacationDay { get; set; }
        public int DiscountTime { get; set; }

        public DiscountTimeClass(string VacationDay, int DiscountTime)
        {
            this.VacationDay = VacationDay;
            this.DiscountTime = DiscountTime;
        }
    }
    public class SalaryReportRepository : ISalaryReportRepository
    {
        private readonly AppDbContext context;

        public SalaryReportRepository(AppDbContext context)
        {
            this.context = context;
        }

        public int CalculateAttendanceDays(List<Attendance> attendances,int id)
        {
            var attendanceCountsByEmployee = attendances
             .Where(a => a.CheckInTime.HasValue && a.CheckOutTime.HasValue && a.EmployeeId == id).Count();
            return attendanceCountsByEmployee;
        }
        public int CalculateAbsentDays(List<Attendance> attendances, int id)
        {
            var attendanceCountsByEmployee = attendances
            .Where(a => !a.CheckInTime.HasValue && !a.CheckOutTime.HasValue && a.EmployeeId == id).Count();

            var generalSetting=context.Set<GeneralSettingsNew>().Include(x=>x.Vacation).ToList();
            var query = generalSetting.Where(x => x.EmployeeId == id).ToList();
            var employeeAttendance= attendances
            .Where(a => !a.CheckInTime.HasValue && !a.CheckOutTime.HasValue && a.EmployeeId == id).ToList();
            foreach (var attendance in employeeAttendance)
            {
                DateTime date = attendance.Date;
                DayOfWeek dayOfWeek = date.DayOfWeek;
                string dayOfWeekString = dayOfWeek.ToString();
                foreach(var item in query)
                {
                    if (dayOfWeekString.Equals(item.Vacation.VacationDay))
                        attendanceCountsByEmployee--;
                }

            }

            return attendanceCountsByEmployee;
        }
        public double GetWorksHours(TimeSpan? checkIn, TimeSpan? checkOut)
        {

            if (checkIn != null || checkOut != null)
            {
                TimeSpan timeDifference = checkOut.Value - checkIn.Value;
                double hoursDifference = timeDifference.TotalHours;
                return hoursDifference;
            }
            return 0;
        }
            
        public int CalculateOvertimeHours(List<Attendance> attendances,int id)
        {
            TimeSpan checkIn=TimeSpan.Zero, CheckOut= TimeSpan.Zero;
            int numOfHours = 0;
            var attendanceEmployeeList = attendances.Where(x=>x.EmployeeId==id&&x.CheckInTime.HasValue&&x.CheckOutTime.HasValue).ToList();
            var employees = context.Set<Employee>().Where(x=>x.Id==id).ToList();
            foreach(var emp in employees)
            {
                checkIn=emp.CheckInTime;
                CheckOut=emp.CheckOutTime;
            }
            List<OverTimeClass> vacations = new List<OverTimeClass>();
            var generalSetting=context.Set<GeneralSettingsNew>().Include(x=>x.Vacation).ToList();
            foreach (var employee in generalSetting)
            {
                if(employee.EmployeeId==id)
                    vacations.Add(new OverTimeClass(employee.Vacation.VacationDay.ToString(),employee.OvertimeHours));
            }
            bool isVacationDay = false;

            foreach (var attendance in attendanceEmployeeList)
            {
                DateTime date = attendance.Date;
                DayOfWeek dayOfWeek = date.DayOfWeek;
                string dayOfWeekString = dayOfWeek.ToString();
                foreach (var vacation in vacations)
                {
                    if (dayOfWeekString.Equals(vacation.VacationDay)&&attendance.CheckInTime<=checkIn && attendance.CheckOutTime >= CheckOut)
                    {
                        int worksHours =(int) GetWorksHours(checkIn, attendance.CheckOutTime);
                        numOfHours += worksHours * (int)vacation.OverTime;
                        isVacationDay=true;
                    }
                    else if (dayOfWeekString.Equals(vacation.VacationDay) && attendance.CheckInTime >= checkIn && attendance.CheckOutTime >= CheckOut)
                    {
                        int worksHours = (int)GetWorksHours(attendance.CheckInTime, attendance.CheckOutTime);
                        numOfHours += worksHours * (int)vacation.OverTime;
                        isVacationDay = true;

                    }

                }
                if (isVacationDay==false && attendance.CheckOutTime> CheckOut)
                {
                    int worksHours = (int)GetWorksHours(CheckOut, attendance.CheckOutTime);
                    numOfHours += worksHours;
                }

            }
           
            return numOfHours;
        }

        public int CalculateDiscountsHours(List<Attendance> attendances, int absentDays, int id)
        {
            TimeSpan checkIn = TimeSpan.Zero, CheckOut = TimeSpan.Zero;
            int worksHour = 0;
            int numOfHours = 0;
            var attendanceEmployeeList = attendances.Where(x => x.EmployeeId == id && x.CheckInTime.HasValue && x.CheckOutTime.HasValue).ToList();
            var employees = context.Set<Employee>().Where(x => x.Id == id).ToList();
            foreach (var emp in employees)
            {
                worksHour = emp.WorkHours.Value;
                checkIn = emp.CheckInTime;
                CheckOut = emp.CheckOutTime;
            }
            List<OverTimeClass> vacations = new List<OverTimeClass>();
            var generalSetting = context.Set<GeneralSettingsNew>().Include(x => x.Vacation).ToList();
            int discountHours = 0;
            foreach (var employee in generalSetting)
            {
                if (employee.EmployeeId == id)
                    discountHours = employee.DiscountHours;
            }
            foreach (var attendance in attendanceEmployeeList)
            {
                if (attendance.CheckInTime > checkIn)
                {
                    int worksHours = (int)GetWorksHours(checkIn, attendance.CheckInTime);
                    numOfHours += worksHours * (int)discountHours;
                }
                if (attendance.CheckOutTime < CheckOut)
                {
                    int worksHours = (int)GetWorksHours(attendance.CheckOutTime, CheckOut);
                    numOfHours += worksHours * (int)discountHours;
                }
                if(attendance.CheckInTime == checkIn && attendance.CheckOutTime == CheckOut)
                    continue;
            }
            return numOfHours+(absentDays* worksHour);
        }

        public decimal CalculateTotalExtra(int overtimeHours, int id)
        {
            decimal hourRate = 0;
            var employee=context.Set<Employee>().Where(x=>x.Id == id).ToList();
            foreach(var emp in employee)
            {
                hourRate = emp.HourRate.Value;
            }
            return hourRate* overtimeHours;
        }

        public decimal CalculateTotalDiscount(int discountHours, int absentDays, int id)
        {
            int totalDiscountHours = 0;
            decimal hourRate = 0;
            int worksHours = 0;
            var employee = context.Set<Employee>().Where(x => x.Id == id).ToList();
            foreach (var emp in employee)
            {
                worksHours=emp.WorkHours.Value;
                hourRate = emp.HourRate.Value;
            }
            totalDiscountHours=discountHours+(absentDays* worksHours);
            return hourRate * totalDiscountHours;
        }

        public decimal CalculateTotalSalary(int attendanceDays, decimal totalExtra, decimal totalDiscount, int id)
        {

            decimal salary = 0,totalSalary=0;
            var employee = context.Set<Employee>().Where(x => x.Id == id).ToList();
            foreach (var emp in employee)
            {
                salary = emp.Salary;
            }
            if(attendanceDays>0) 
            {
                totalSalary=salary+totalExtra- totalDiscount;
                return totalSalary;   
            }
            return 0;
        }
        public List<SalaryNewReport> Search(string name,DateTime startDate, DateTime endDate)
        {
            var employees = context.Employees.Where(x =>
            x.Name.Trim().ToLower().Contains(name.Trim().ToLower()) 
            ).ToList();
            var attendances = context.Attendances.Where(a => a.Date >= startDate && a.Date <= endDate)
               .Include(c => c.Employee).ToList();
            var employeesQuery = from e in employees
                                 join a in attendances
                                 on e.Id equals a.EmployeeId
                                 select e;
            var filterEmployees = employeesQuery.Distinct().ToList();
            return GetSalaryNewReports(filterEmployees, attendances);
        }
        public List<SalaryNewReport> SearchbyDate(DateTime startDate, DateTime endDate)
        {
            var employees=context.Employees.ToList();
            var attendances = context.Attendances.Where(a => a.Date >= startDate && a.Date <= endDate)
                .Include(c => c.Employee).ToList();

            var employeesQuery =from e in employees
                               join a in attendances
                               on e.Id equals a.EmployeeId
                               select e;
            var filterEmployees=employeesQuery.Distinct().ToList();
            return GetSalaryNewReports(filterEmployees, attendances);
        }

        public List<SalaryNewReport> GetSalaryNewReports(List<Employee> employees, List<Attendance> listOfAttendance)
        {
            List<SalaryNewReport> model = new List<SalaryNewReport>();

            foreach (var employee in employees)
            {
                int attendanceDays = CalculateAttendanceDays(listOfAttendance, employee.Id);
                int absentDays = CalculateAbsentDays(listOfAttendance, employee.Id);
                int overTimeHours = CalculateOvertimeHours(listOfAttendance, employee.Id);
                int discountHours = CalculateDiscountsHours(listOfAttendance, absentDays, employee.Id);
                decimal totalExtra = CalculateTotalExtra(overTimeHours, employee.Id);
                decimal totalDiscount = CalculateTotalDiscount(discountHours, absentDays, employee.Id);
                decimal totalSalary = CalculateTotalSalary(attendanceDays, totalExtra, totalDiscount, employee.Id);
                var mm = new SalaryNewReport()
                {
                    AbsentDays = absentDays,
                    TotalDiscountHours = discountHours,
                    AttendanceDays = attendanceDays,
                    Employee = employee,
                    TotalDiscount = totalDiscount,
                    TotalExtra = totalExtra,
                    TotalOverTimeHours = overTimeHours,
                    TotalSalary = totalSalary

                };
                model.Add(mm);

            }

            return model;
        }
    }
}
