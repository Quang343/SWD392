using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly TimesheetDbContext _context;

        public ApprovalRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public Timesheet GetTimesheetById(int id)
        {
            return _context.Timesheets.FirstOrDefault(t => t.TimesheetId == id);
        }

        public void UpdateTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Update(timesheet);
        }

        public void AddApproval(Approval approval)
        {
            _context.Approvals.Add(approval);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
