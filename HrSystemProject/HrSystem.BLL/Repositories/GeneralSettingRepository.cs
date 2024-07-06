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

namespace HRSystem.BLL.Repositories
{
    public class GeneralSettingRepository:IGeneralSettingRepository
    {
        private readonly AppDbContext context;

        public GeneralSettingRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<GeneralSettingsNew> GetAll()
        {
            return context.Set<GeneralSettingsNew>().Include(x=>x.Employee).Include(x=>x.Vacation).ToList();
        }
        public void Add(GeneralSettingsNew generalSetting)
        {
            context.GeneralSettingsNew.Add(generalSetting);
        }

        public GeneralSettingsNew GetById(int employeeId)
        {
            return context.Set<GeneralSettingsNew>()
                              .FirstOrDefault(x => x.EmployeeId == employeeId);
        }
        public List<GeneralSettingsNew> GetByIdList(int employeeId)
        {
            return context.Set<GeneralSettingsNew>()
                              .Where(x => x.EmployeeId == employeeId).Include(x=>x.Employee).ToList();
        }
        public void Update(GeneralSettingsNew generalSetting)
        {
            context.Update(generalSetting);
        }
        public void Delete(GeneralSettingsNew generalSetting)
        {
            var genSetting= context.Set<GeneralSettingsNew>()
                              .Where(x => x.EmployeeId == generalSetting.EmployeeId).ToList();
            foreach (var item in genSetting)
                context.Remove(item);
        }
        
        public IEnumerable<GeneralSettingDto> Search(List<GeneralSettingDto> model, string name)
        {
            List<GeneralSettingDto> generalSettingDto = new List<GeneralSettingDto>();
            GeneralSettingDto mappingDto;

            foreach (var item in model)
            {
                if(item.Employee.Name == name)
                {
                    mappingDto = new GeneralSettingDto()
                    {
                        EmployeeId = item.EmployeeId,
                        DiscountHours = item.DiscountHours,
                        Employee = item.Employee,
                        OvertimeHours = item.OvertimeHours,
                        Vacations = item.Vacations 
                    };
                    generalSettingDto.Add(mappingDto);
                }
            }    
            return generalSettingDto;
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            var generalSetting = context.GeneralSettingsNew.Select(c=>c.EmployeeId).ToList();

            var employees = context.Employees.Where(c=> !generalSetting.Contains(c.Id)).ToList();
            return employees;
        }

        public IEnumerable<GeneralSettingDto> MappingGeneralSetting(List<GeneralSettingsNew> model)
        {
           List<GeneralSettingDto> generalSettingDto = new List<GeneralSettingDto>();
            if(model != null && model.Count >0)
            {
                foreach (var item in model)
                {
                    GeneralSettingDto mappingDto;
                    var temp = generalSettingDto.FirstOrDefault(c => c.EmployeeId == item.EmployeeId);
                    if (temp == null)
                    {
                        mappingDto = new GeneralSettingDto
                        {
                            EmployeeId = item.EmployeeId,
                            DiscountHours = item.DiscountHours,
                            Employee = item.Employee,
                            OvertimeHours = item.OvertimeHours,
                            Vacations = item.Vacation.VacationDay,
                            
                        };
                        generalSettingDto.Add(mappingDto);
                    }
                    else
                    {
                        temp.Vacations = temp.Vacations + ","+ item.Vacation.VacationDay;
                    }
                }
            }
            return generalSettingDto;
            

        }

    }
}
