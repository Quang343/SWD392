using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public interface ITimesheetRepository
    {
        List<Timesheet> GetAll();
        List<TaskItem> GetTasksByEmployeeId(int employeeId);
        List<TaskItem> GetAllTasks();
        Timesheet GetTimesheetByWeek(int employeeId, DateTime startDate);
        void CreateTimesheet(Timesheet timesheet);
        void UpdateTimesheet(Timesheet timesheet);
        void DeleteEntries(List<TimesheetEntry> entries);
        List<Timesheet> GetTimesheetsByEmployeeId(int employeeId);
        Timesheet GetTimesheetById(int timesheetId);
        void DeleteTimesheet(Timesheet timesheet);
    }
}
