using AutoMapper;
using Hr.System.PL.Models;
using HrSystem.BLL.Interfaces;
using HrSystem.BLL.Repositories;
using HrSystem.DAL.Entities;
using HrSystem.PL.Helper;
using HrSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HrSystem.PL.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        public List<Attendance> Attendances;

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AttendanceController> logger;
        private readonly IMapper mapper;

        public AttendanceController(IUnitOfWork unitOfWork, ILogger<AttendanceController> logger, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mapper = mapper;
        }

        public IActionResult GetAttandancesList(int? pageNumber, int? pageSize, string SearchValue = "")
        {
            IEnumerable<Attendance> listOfAttendance;
            ViewBag.Employees = unitOfWork.EmployeeRepository.GetAll();
            if (string.IsNullOrEmpty(SearchValue))
            {
                listOfAttendance = unitOfWork.AttendanceRepository.GetAll();
            }
            else
            {
                listOfAttendance = unitOfWork.AttendanceRepository.Search(SearchValue);
            }
            int defaultPageSize = 10;
            int currentPageSize = pageSize ?? defaultPageSize;
            int currentPageNumber = pageNumber ?? 1;

            var paginatedList = PaginatedList<Attendance>.CreateAsync(listOfAttendance.AsQueryable(), currentPageNumber, currentPageSize);

            AttendanceViewModel model = new AttendanceViewModel()
            {
                Attendance = new Attendance { Date = DateTime.Now },
                ListOfAttendance = paginatedList.ToList()
            };
            ViewBag.PaginatedList = paginatedList;

            return View(model);
        }
        [HttpGet]
        public IActionResult SingleAttendance()
        {
            ViewBag.Employees = unitOfWork.EmployeeRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult SingleAttendance(AttendanceViewModel model)
        {
            if (model.Attendance.CheckOutTime != null)
            {
                if (model.Attendance.CheckOutTime < model.Attendance.CheckInTime)
                {
                    ModelState.AddModelError(string.Empty, "Check time out can not be less than check in");
                    IEnumerable<Attendance> listOfAttendance;
                    ViewBag.Employees = unitOfWork.EmployeeRepository.GetAll();

                    listOfAttendance = unitOfWork.AttendanceRepository.GetAll();

                    Attendance attmodel = new Attendance
                    {
                        Date = DateTime.Now
                    };
                    AttendanceViewModel modeltt = new AttendanceViewModel();
                    modeltt.Attendance = model.Attendance;
                    modeltt.ListOfAttendance = listOfAttendance.ToList();

                    return View(nameof(GetAttandancesList), modeltt);
                }
            }

            unitOfWork.AttendanceRepository.Add(model.Attendance);
            unitOfWork.Complete();
            return RedirectToAction("GetAttandancesList");

        }
        [HttpGet]
        public IActionResult BatchAttendance()
        {
            var updatedListJson = TempData["UpdatedListOfAttendance"] as string;
            List<Attendance> attendances = string.IsNullOrEmpty(updatedListJson)
                                            ? new List<Attendance>()
                                            : JsonConvert.DeserializeObject<List<Attendance>>(updatedListJson);

            AttendanceViewModel modeltt = new AttendanceViewModel();
            modeltt.ListOfAttendance = attendances;
            return View(modeltt);
        }
        [HttpGet]
        public IActionResult UpdateEmployeeAttendanceInSheet(int id, DateTime date)
        {
            List<Attendance> Attends = Attendances;
            AttendanceViewModel newModel = new AttendanceViewModel();

            var attendance = Attends.FirstOrDefault(a => a.EmployeeId == id && a.Date == date);
            if (attendance != null)
            {
                newModel.Attendance = attendance;
            }

            return View(newModel);
        }
        [HttpPost]
        public IActionResult UpdateEmployeeAttendanceInSheet(AttendanceViewModel model)
        {
            if (model.Attendance.CheckOutTime < model.Attendance.CheckInTime)
            {
                ModelState.AddModelError(string.Empty, "Check-out time cannot be earlier than check-in time.");
                return View(model);
            }

            var attendance = model.ListOfAttendance.FirstOrDefault(a => a.EmployeeId == model.Attendance.EmployeeId && a.Date == model.Attendance.Date);
            if (attendance == null)
            {
                return NotFound();
            }

            attendance.CheckInTime = model.Attendance.CheckInTime;
            attendance.CheckOutTime = model.Attendance.CheckOutTime;

            
            TempData["UpdatedListOfAttendance"] = JsonConvert.SerializeObject(model.ListOfAttendance);

            return RedirectToAction("BatchAttendance");
        }
        [HttpPost]
        public IActionResult BatchAttendance(AttendanceViewModel model)
        {

            foreach (Attendance attendance in model.ListOfAttendance)
            {
                if (attendance.CheckOutTime != null && attendance.CheckOutTime < attendance.CheckInTime)
                {
                    ModelState.AddModelError(string.Empty, $"Employee with Id [{attendance.EmployeeId}] - and Day[{attendance.Date.ToShortDateString()}] Check-out time {attendance.CheckOutTime} cannot be earlier than check-in time {attendance.CheckInTime}.");
                    return View("BatchAttendance", model);
                }
                unitOfWork.AttendanceRepository.Add(attendance);
            }
            unitOfWork.Complete();
            return RedirectToAction("GetAttandancesList");
        }
        public IActionResult DownloadTemplate()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/AttTemplete", "Template.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Template.xlsx");
        }
        [HttpPost]
        public IActionResult UploadFilledTemplate(UploadViewModel model)
        {
            AttendanceViewModel modeltt = new AttendanceViewModel();
            List<Attendance> attendances = new List<Attendance>();
            if (model.File == null || model.File.Length == 0)
            {
                return Content("File not selected");

            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                model.File.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Attendance attendance = new Attendance();

                        attendance.EmployeeId = int.Parse(worksheet.Cells[row, 1].Text);
                        attendance.Date = DateTime.Parse(worksheet.Cells[row, 2].Text);
                        attendance.CheckInTime = TimeSpan.TryParse(worksheet.Cells[row, 3].Text, out TimeSpan checkInTime) ? checkInTime : (TimeSpan?)null;
                        attendance.CheckOutTime = TimeSpan.TryParse(worksheet.Cells[row, 4].Text, out TimeSpan checkOutTime) ? checkOutTime : (TimeSpan?)null;

                        attendances.Add(attendance);
                    }
                    
                }
            }
            Attendances = attendances;
            modeltt.ListOfAttendance= attendances;
            return View("BatchAttendance",modeltt);
        }
        [HttpGet]
        public IActionResult UpdateEmployeeAttendance(int id)
        {
            ViewBag.Employees = unitOfWork.EmployeeRepository.GetAll();
            var attendance = unitOfWork.AttendanceRepository.GetById(id);
            AttendanceViewModel model = new AttendanceViewModel();
            model.Attendance = attendance;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateEmployeeAttendance(AttendanceViewModel model)
        {
            if (model.Attendance.CheckOutTime != null)
            {
                if (model.Attendance.CheckOutTime < model.Attendance.CheckInTime)
                {
                    ModelState.AddModelError(string.Empty, "Check time out can not be less than check in");
                    ViewBag.Employees = unitOfWork.EmployeeRepository.GetAll();
                    var attendance = unitOfWork.AttendanceRepository.GetById(model.Attendance.Id);
                    AttendanceViewModel modelnew = new AttendanceViewModel();
                    modelnew.Attendance = attendance;
                    return View(nameof(UpdateEmployeeAttendance), modelnew);
                }
            }
            unitOfWork.AttendanceRepository.Update(model.Attendance);
            unitOfWork.Complete();
            return RedirectToAction("GetAttandancesList");
        }
        public IActionResult DeleteEmployeeAttendance(int id)
        {
            var attendance = unitOfWork.AttendanceRepository.GetById(id);
            unitOfWork.AttendanceRepository.Delete(attendance);
            unitOfWork.Complete();
            return RedirectToAction("GetAttandancesList");
        }
        
    }

}
