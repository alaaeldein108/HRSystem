using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Context;
using HrSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.BLL.Repositories
{
    public class VacationRepository : IVacationRepository
    {
        private readonly AppDbContext context;

        public VacationRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Vacation> GetAll()
        {
            return context.Vacations.ToList();
        }
    }
}
