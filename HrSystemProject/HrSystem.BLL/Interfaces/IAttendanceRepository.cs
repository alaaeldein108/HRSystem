using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Interfaces
{
    public interface IAttendanceRepository
    {
        Attendance GetById(int id);
        IEnumerable<Attendance> Search(string name);
        void Add(Attendance attendance);
        IEnumerable<Attendance> GetAll();
        void Delete(Attendance attendance);
        void Update(Attendance attendance);


    }
}
