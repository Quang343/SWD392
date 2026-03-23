
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

        public List<TaskItem> GetTasksByEmployeeId(int employeeId)
        {
            // Join Assignments -> Projects -> Tasks
            return _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.ProjectAssignments.Any(pa => pa.EmployeeId == employeeId))
                .ToList();
        }

        public List<TaskItem> GetAllTasks()
        {
            return _context.Tasks.Include(t => t.Project).ToList();
        }

        public Timesheet GetTimesheetByWeek(int employeeId, DateTime startDate)
        {
            return _context.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefault(t => t.EmployeeId == employeeId && t.WeekStartDate == startDate);
        }

        public void CreateTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            _context.SaveChanges();
        }

        public void UpdateTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Update(timesheet);
            _context.SaveChanges();
        }

        public void DeleteEntries(List<TimesheetEntry> entries)
        {
            _context.TimesheetEntries.RemoveRange(entries);
            _context.SaveChanges();
        }
    }
}
