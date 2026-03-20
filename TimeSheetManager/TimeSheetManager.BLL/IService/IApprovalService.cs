using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetManager.BLL.IService
{
    public interface IApprovalService
    {
        void Approve(int timesheetId, int managerUserId, string comment);
        void Reject(int timesheetId, int managerUserId, string comment);
    }
}
