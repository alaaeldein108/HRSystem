using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Hr.System.PL.Controllers
{
    public class InvoiceController : Controller
    {
        
        public IActionResult CreateInvoice(int id, string name, string phone, decimal salary, decimal hourRate,
            int attendanceDays, int absentDays, decimal totalOverTimeHours, decimal totalDiscountHours,
            decimal totalExtra, decimal totalDiscount, decimal totalSalary)
        {
            var model = new SalaryNewReport
            {
                Employee = new Employee
                {
                    Id = id,
                    Name = name,
                    PhoneNumber = phone,
                    Salary = salary,
                    HourRate = hourRate
                },
                AttendanceDays = attendanceDays,
                AbsentDays = absentDays,
                TotalOverTimeHours = totalOverTimeHours,
                TotalDiscountHours = totalDiscountHours,
                TotalExtra = totalExtra,
                TotalDiscount = totalDiscount,
                TotalSalary = totalSalary
            };

            return View(model);
        }

    }
}
