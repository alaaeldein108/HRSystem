using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Entities
{
    public class Vacation
    {
        public int Id { get; set; }
        public string VacationDay { get; set; }
        public ICollection<GeneralSettingsNew> GeneralSettings { get; set; } = new List<GeneralSettingsNew>();

    }
}
