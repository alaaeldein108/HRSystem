using AutoMapper;
using HrSystem.DAL.Entities;
using HrSystem.PL.Models;

namespace HRSystem.PL.Mapper
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }

    }
}
