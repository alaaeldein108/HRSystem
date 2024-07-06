using AutoMapper;
using HrSystem.BLL.Interfaces;
using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;
using HrSystem.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.PL.Controllers
{
    [Authorize]
    public class SalaryReportController : Controller
    {
		private readonly IUnitOfWork unitOfWork;
		private readonly ILogger<SalaryReportController> logger;
		private readonly IMapper mapper;

		public SalaryReportController(IUnitOfWork unitOfWork, ILogger<SalaryReportController> logger, IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.logger = logger;
			this.mapper = mapper;
		}
		public IActionResult GetReport(DateTime? fromDate, DateTime? toDate,string SearchValue = "")
        {
            if(fromDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }

            if (toDate == null)
            {
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            }
            

            List<SalaryNewReport> listOfSalaryReport=new List<SalaryNewReport>();
			if (string.IsNullOrEmpty(SearchValue))
			{
                listOfSalaryReport = unitOfWork.SalaryReportRepository.SearchbyDate(fromDate.Value, toDate.Value);
            }
            else
			{
				listOfSalaryReport = unitOfWork.SalaryReportRepository.Search(SearchValue, fromDate.Value, toDate.Value);
			}
			SalaryreportViewModel model=new SalaryreportViewModel();
			model.ListOfSalaryReport = listOfSalaryReport;
            model.FromDate=fromDate.Value;
            model.ToDate=toDate.Value;
			return View(model);
        }
       
    }
}
