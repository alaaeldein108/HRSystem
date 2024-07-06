using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee GetById(int id);
        IEnumerable<Employee> Search(string name);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        IEnumerable<Employee> GetAll();
        decimal CalculateHourRate(decimal salary, TimeSpan CheckInTime, TimeSpan CheckOutTime);


    }
}
