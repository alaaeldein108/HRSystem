using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Context;
using HrSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Repositories
{
    public class AttendanceRepository:IAttendanceRepository
    {
        private readonly AppDbContext context;

        public AttendanceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(Attendance attendance)
        {
            context.Attendances.Add(attendance);
        }
        public void Update(Attendance attendance)
        {
            context.Set<Attendance>().Update(attendance);
        }
        public void Delete(Attendance attendance)
        {
             context.Attendances.Remove(attendance);
        }

        public IEnumerable<Attendance> GetAll()
        {
            return context.Set<Attendance>().Include(c=>c.Employee).ToList();
        }

        public Attendance GetById(int id)
        {
            return context.Attendances.FirstOrDefault(x=>x.Id==id);
        }

        public IEnumerable<Attendance> Search(string name)
        {
            var query = from E in context.Employees
                        where E.Name.Trim().ToLower() == name.Trim().ToLower()
                        select E.Id;
            var employeeIds= query.ToList();

            return context.Attendances.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();
        }

        
    }
}
