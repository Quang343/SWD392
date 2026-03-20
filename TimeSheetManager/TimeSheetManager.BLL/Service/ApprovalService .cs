using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.IService;
using TimeSheetManager.DAL.Repository;
using TimeSheetManager.Models;

namespace TimeSheetManager.BLL.Service
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _repo;

        public ApprovalService(IApprovalRepository repo)
        {
            _repo = repo;
        }

        public void Approve(int timesheetId, int managerUserId, string comment)
        {
            var timesheet = _repo.GetTimesheetById(timesheetId);

            if (timesheet == null)
                throw new Exception("Timesheet not found");

            if (timesheet.Status != "Pending")
                throw new Exception("Only pending timesheet can be approved");

            timesheet.Status = "Approved";

            _repo.UpdateTimesheet(timesheet);

            _repo.AddApproval(new Approval
            {
                TimesheetId = timesheetId,
                ApprovalDate = DateTime.Now,
                Status = "Approved",
                Comment = comment,
                ApprovedByUserId = managerUserId
            });

            _repo.Save();
        }

        public void Reject(int timesheetId, int managerUserId, string comment)
        {
            var timesheet = _repo.GetTimesheetById(timesheetId);

            if (timesheet == null)
                throw new Exception("Timesheet not found");

            if (timesheet.Status != "Pending")
                throw new Exception("Only pending timesheet can be rejected");

            timesheet.Status = "Rejected";

            _repo.UpdateTimesheet(timesheet);

            _repo.AddApproval(new Approval
            {
                TimesheetId = timesheetId,
                ApprovalDate = DateTime.Now,
                Status = "Rejected",
                Comment = comment,
                ApprovedByUserId = managerUserId
            });

            _repo.Save();
        }
    }
}
