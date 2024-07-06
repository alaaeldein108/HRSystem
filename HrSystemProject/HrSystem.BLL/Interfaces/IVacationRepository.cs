using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Interfaces
{
    public interface IVacationRepository
    {
        IEnumerable<Vacation> GetAll(); 
    }
}
