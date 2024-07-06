using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using System.Drawing.Printing;

namespace HrSystem.PL.Models
{
    public class GeneralSettingViewModel
    {
        public GeneralSettingsNew GeneralSettings { get; set; }
        public List<GeneralSettingsNew> ListOfGeneralSettings { get; set; }
        public List<Vacation> ListOfVacation { get; set; }
        public List<GeneralSettingDto> ListOfGeneralSettingsDto { get; set; }
        public List<EmployeeVacationDto> EmployeeVacation { get; set; }
    }
}
