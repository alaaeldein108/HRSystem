using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrSystem.DAL.Identity
{
    public static class Permissions
    {
        public const string AddEmployee = "AddEmployee";
        public const string ViewEmployee = "ViewEmployee";
        public const string EditEmployee = "EditEmployee";
        public const string DeleteEmployee = "DeleteEmployee";

        public const string AddGeneralSetting = "AddGeneralSetting";
        public const string ViewGeneralSetting = "ViewGeneralSetting";
        public const string EditGeneralSetting = "EditGeneralSetting";
        public const string DeleteGeneralSetting = "DeleteGeneralSetting";

        public const string AddAttendance = "AddAttendance";
        public const string ViewAttendance = "ViewAttendance";
        public const string EditAttendance = "EditAttendance";
        public const string DeleteAttendance = "DeleteAttendance";

        public const string AddSingleAttendance = "AddSingleAttendance";

        public const string AddBatchAttendance = "AddBatchAttendance";

        public const string ViewSalaryReport = "ViewSalaryReport";

        public const string ViewInvoice = "ViewInvoice";

        public const string AddUser = "AddUser";
        public const string ViewUser = "ViewUser";
        public const string EditUser = "EditUser";
        public const string DeleteUser = "DeleteUser";

        public const string AddRole = "AddRole";
        public const string ViewRole = "ViewRole";
        public const string EditRole = "EditRole";
        public const string DeleteRole = "DeleteRole";

    }
}
