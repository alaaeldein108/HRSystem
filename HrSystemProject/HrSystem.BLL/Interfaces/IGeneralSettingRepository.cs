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
    public interface IGeneralSettingRepository
    {
        public IEnumerable<GeneralSettingsNew> GetAll();
        public void Add(GeneralSettingsNew generalSetting);
        public List<GeneralSettingsNew> GetByIdList(int employeeId);
        public GeneralSettingsNew GetById(int employeeId);

        public void Update(GeneralSettingsNew generalSetting);
        public void Delete(GeneralSettingsNew generalSetting);

        IEnumerable<GeneralSettingDto> Search(List<GeneralSettingDto> model, string name);

        public IEnumerable<Employee> GetAllEmployees();
        public IEnumerable<GeneralSettingDto> MappingGeneralSetting(List<GeneralSettingsNew> model);

    }
}
