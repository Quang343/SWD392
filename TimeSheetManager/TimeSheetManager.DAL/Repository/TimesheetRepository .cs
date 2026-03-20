
using Microsoft.EntityFrameworkCore;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly TimesheetDbContext _context;

        public TimesheetRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public List<Timesheet> GetAll()
        {
            return _context.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Entries)
                    .ThenInclude(e => e.Task)
                .ToList();
        }
    }
}
