using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public interface IApprovalRepository
    {
        Timesheet GetTimesheetById(int id);
        void UpdateTimesheet(Timesheet timesheet);
        void AddApproval(Approval approval);
        void Save();
    }
}
