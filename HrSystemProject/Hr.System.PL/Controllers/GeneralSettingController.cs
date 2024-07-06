using AutoMapper;
using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using HrSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HRSystem.PL.Controllers
{
    [Authorize]
    public class GeneralSettingController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<GeneralSettingController> logger;
        private readonly IMapper mapper;

        public GeneralSettingController(IUnitOfWork unitOfWork, ILogger<GeneralSettingController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }
        public IActionResult CreateGeneralSetting()
        {
            var vacation=unitOfWork.VacationRepository.GetAll();

            ViewBag.Employees = unitOfWork.GeneralSettingRepository.GetAllEmployees();
            var model=new GeneralSettingViewModel();
            model.ListOfVacation = vacation.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateGeneralSetting(GeneralSettingViewModel model, int[] selectedVacations)
        {
           
            foreach(var vacationId in selectedVacations)
            {
                var generalSetting = new GeneralSettingsNew 
                {
                    EmployeeId=model.GeneralSettings.EmployeeId,
                    DiscountHours=model.GeneralSettings.DiscountHours,
                    OvertimeHours=model.GeneralSettings.OvertimeHours,
                    VacationId=vacationId,
                };
                unitOfWork.GeneralSettingRepository.Add(generalSetting);

            }
            unitOfWork.Complete();
            return RedirectToAction("GetAllGeneralSetting");
           
        }
        public IActionResult GetAllGeneralSetting(string SearchValue = "")
        {
            List<GeneralSettingDto> generalSettings;
            GeneralSettingViewModel model = new GeneralSettingViewModel();

            if (string.IsNullOrEmpty(SearchValue))
            {
                var listOfGeneralSettings=unitOfWork.GeneralSettingRepository.GetAll().ToList();
                generalSettings = unitOfWork.GeneralSettingRepository.MappingGeneralSetting(listOfGeneralSettings).ToList();
            }
            else
            {
                var listOfGeneralSettings = unitOfWork.GeneralSettingRepository.GetAll().ToList();

                generalSettings = unitOfWork.GeneralSettingRepository.MappingGeneralSetting(listOfGeneralSettings).ToList();

                generalSettings = unitOfWork.GeneralSettingRepository.Search(generalSettings, SearchValue).ToList();

            }
            model.ListOfGeneralSettingsDto= generalSettings;
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateGeneralSetting(int employeeId)
        {
            int empId=0, overTimeHours=0, discountHours = 0;
            var vacations = unitOfWork.VacationRepository.GetAll();
            var generalSettingRecord = unitOfWork.GeneralSettingRepository.GetByIdList(employeeId);
            List<EmployeeVacationDto> employeeVacations=new List<EmployeeVacationDto>();
            GeneralSettingViewModel model = new GeneralSettingViewModel();
           
            foreach(var item in generalSettingRecord)
            {
                EmployeeVacationDto vacation = new EmployeeVacationDto()
                {
                    EmployeeId=item.EmployeeId,
                    VacationId=item.VacationId,
                    value=true
                };
                employeeVacations.Add(vacation);
                empId=item.EmployeeId;
                overTimeHours = item.OvertimeHours;
                discountHours = item.DiscountHours;
            }
            var generalStetting = new GeneralSettingsNew() 
            {
                EmployeeId=empId,
                OvertimeHours=overTimeHours,
                DiscountHours=discountHours,
            };
            model.ListOfGeneralSettings = generalSettingRecord;
            model.EmployeeVacation= employeeVacations;
            model.ListOfVacation = vacations.ToList();
            model.GeneralSettings = generalStetting;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateGeneralSetting(GeneralSettingViewModel model, int[] selectedVacations)
        {
            DeleteGeneralSetting(model.GeneralSettings.EmployeeId);

            foreach (var vacationId in selectedVacations)
            {
                var generalSetting = new GeneralSettingsNew
                {
                    EmployeeId = model.GeneralSettings.EmployeeId,
                    DiscountHours = model.GeneralSettings.DiscountHours,
                    OvertimeHours = model.GeneralSettings.OvertimeHours,
                    VacationId = vacationId,
                };
                unitOfWork.GeneralSettingRepository.Add(generalSetting);

            }

            unitOfWork.Complete();
            return RedirectToAction("GetAllGeneralSetting");
        }
        public IActionResult DeleteGeneralSetting(int employeeId)
        {
            var generalSettingRecord = unitOfWork.GeneralSettingRepository.GetById(employeeId);
            unitOfWork.GeneralSettingRepository.Delete(generalSettingRecord);
            unitOfWork.Complete();
            return RedirectToAction("GetAllGeneralSetting");
        }
    }
}
