using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Context;
using HrSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.BLL.Repositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(Employee employee)
        {
            context.Employees.Add(employee);
        }

        public decimal CalculateHourRate(decimal salary, TimeSpan CheckInTime, TimeSpan CheckOutTime)
        {
            TimeSpan timeDifference = CheckOutTime - CheckInTime;
            double workHours = timeDifference.TotalHours;
           return (salary / 30) / (decimal)workHours;
        }

        public void Delete(Employee employee)
        {
            context.Employees.Remove(employee);
        }

        public IEnumerable<Employee> GetAll()
        {

            return context.Set<Employee>().ToList();
        }

        public Employee GetById(int id)
        {
            return context.Set<Employee>().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Employee> Search(string name)
        {
            var result = context.Employees.Where(x =>
            x.Name.Trim().ToLower().Contains(name.Trim().ToLower()) ||
            x.Email.Trim().ToLower().Contains(name.Trim().ToLower())
            );
            return result;
        }

        public void Update(Employee employee)
        {
            context.Employees.Update(employee);
        }
    }
}
