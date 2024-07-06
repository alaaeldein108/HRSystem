using AutoMapper;
using HrSystem.BLL.Interfaces;
using HrSystem.BLL.Repositories;
using HrSystem.DAL.Entities;
using HrSystem.DAL.Identity;
using HrSystem.PL.Helper;
using HrSystem.PL.Models;
using HRSystem.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace HrSystem.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<EmployeeController> logger;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger
            , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }
        public IActionResult EmployeeInformation(int? pageNumber, int? pageSize, string SearchValue = "")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesViewModel;

            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = unitOfWork.EmployeeRepository.Search(SearchValue);
            }
            int totalEmployees = employees.Count();
            employeesViewModel = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            int defaultPageSize = 10;

            var paginatedList = PaginatedList<EmployeeViewModel>.CreateAsync(employeesViewModel.AsQueryable(), pageNumber ?? 1, pageSize ?? defaultPageSize);

            return View(paginatedList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                if(employeeViewModel.Image == null)
                {
                    ModelState.AddModelError(string.Empty, "Img is Required");
                    return View(employeeViewModel);
                }
                var employee = mapper.Map<Employee>(employeeViewModel);
                employee.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "Images");
                employee.HourRate = unitOfWork.EmployeeRepository.CalculateHourRate(employeeViewModel.Salary, employeeViewModel.CheckInTime, employeeViewModel.CheckOutTime);
                unitOfWork.EmployeeRepository.Add(employee);
                unitOfWork.Complete();
                return RedirectToAction("EmployeeInformation");
            }
            return View(employeeViewModel);
        }
        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            try
            {

                var employee = unitOfWork.EmployeeRepository.GetById(id);
                var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var employee = mapper.Map<Employee>(employeeViewModel);
                if(employeeViewModel.Image != null) {
                    employee.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "Images");
                }
                if (string.IsNullOrEmpty(employeeViewModel.ImageUrl))
                {
                    ModelState.AddModelError(string.Empty, "Image is Required");
                    return View(employeeViewModel);
                }
                employee.HourRate = unitOfWork.EmployeeRepository.CalculateHourRate(employeeViewModel.Salary, employeeViewModel.CheckInTime, employeeViewModel.CheckOutTime);
                unitOfWork.EmployeeRepository.Update(employee);
                unitOfWork.Complete();
                return RedirectToAction(nameof(EmployeeInformation)); ;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var employee = unitOfWork.EmployeeRepository.GetById(id);
                var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult DeleteEmployee(EmployeeViewModel employeeViewModel)
        {
            string imageName = employeeViewModel.ImageUrl;
            var employee = mapper.Map<Employee>(employeeViewModel);
            unitOfWork.EmployeeRepository.Delete(employee);
            unitOfWork.Complete();
            DocumentSetting.DeleteFile(imageName, "Images");
            return RedirectToAction(nameof(EmployeeInformation));
        }
    }
}
