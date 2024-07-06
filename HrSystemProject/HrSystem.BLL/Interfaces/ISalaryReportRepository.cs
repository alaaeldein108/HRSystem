using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Interfaces
{
    public interface ISalaryReportRepository
    {
        
        int CalculateAttendanceDays(List<Attendance> attendances,int id);
        int CalculateAbsentDays(List<Attendance> attendances, int id);

        int CalculateOvertimeHours(List<Attendance> attendances,int id);
        int CalculateDiscountsHours(List<Attendance> attendances,int absentDays, int id);
        decimal CalculateTotalExtra(int overtimeHours,int id);
        decimal CalculateTotalDiscount(int discountHours, int absentDays, int id);
        decimal CalculateTotalSalary(int attendanceDays, decimal totalExtra, decimal totalDiscount, int id);
        double GetWorksHours(TimeSpan? checkIn,TimeSpan? checkOut);
        List<SalaryNewReport> Search(string name, DateTime startDate, DateTime endDate);
        List<SalaryNewReport> SearchbyDate(DateTime startDate, DateTime endDate);
        List<SalaryNewReport> GetSalaryNewReports(List<Employee> employees, List<Attendance> listOfAttendance);

    }
}
