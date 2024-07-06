using HrSystem.DAL.Entities;
using HrSystem.DAL.ViewModel;

namespace HrSystem.PL.Models
{
	public class SalaryreportViewModel
	{
		public SalaryNewReport SalaryReport { get; set; }
		public List<SalaryNewReport> ListOfSalaryReport { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
