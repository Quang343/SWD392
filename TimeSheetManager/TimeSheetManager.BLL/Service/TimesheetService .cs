using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.BLL.IService;
using TimeSheetManager.DAL.Repository;
using TimeSheetManager.Models;

namespace TimeSheetManager.BLL.Service
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repo;
        private readonly IEmployeeRepository _empRepo;

        public TimesheetService(ITimesheetRepository repo, IEmployeeRepository empRepo)
        {
            _repo = repo;
            _empRepo = empRepo;
        }

        public List<TimesheetDTO> GetAll()
        {
            var data = _repo.GetAll();

            return data.Select(t => new TimesheetDTO
            {
                TimesheetId = t.TimesheetId,
                EmployeeName = t.Employee.FullName,
                Status = t.Status,
                Entries = t.Entries.Select(e => new TimesheetEntryDTO
                {
                    WorkDate = e.WorkDate,
                    HoursWorked = e.HoursWorked,
                    TaskName = e.Task.TaskName
                }).ToList()
            }).ToList();
        }

        private int GetEmployeeId(string username)
        {
            var emp = _empRepo.GetByUsername(username);
            if (emp == null) throw new Exception("Không tìm thấy Employee cho Username: " + username);
            return emp.EmployeeId;
        }

        public List<MyTaskDTO> GetMyTasks(string username)
        {
            var empId = GetEmployeeId(username);
            var tasks = _repo.GetTasksByEmployeeId(empId);
            return tasks.Select(t => new MyTaskDTO
            {
                TaskItemId = t.TaskItemId,
                TaskName = t.TaskName,
                ProjectName = t.Project.ProjectName
            }).ToList();
        }

        public List<MyTaskDTO> GetAvailableTasks()
        {
            var tasks = _repo.GetAllTasks();
            return tasks.Select(t => new MyTaskDTO
            {
                TaskItemId = t.TaskItemId,
                TaskName = t.TaskName,
                ProjectName = t.Project.ProjectName
            }).ToList();
        }

        public MyTimesheetDTO GetMyTimesheet(string username, DateTime startDate)
        {
            var empId = GetEmployeeId(username);
            var timesheet = _repo.GetTimesheetByWeek(empId, startDate);
            if (timesheet == null) return null;

            return new MyTimesheetDTO
            {
                TimesheetId = timesheet.TimesheetId,
                WeekStartDate = timesheet.WeekStartDate,
                WeekEndDate = timesheet.WeekEndDate,
                Status = timesheet.Status,
                Entries = timesheet.Entries.Select(e => new MyTimesheetEntryDTO
                {
                    TimesheetEntryId = e.TimesheetEntryId,
                    TaskItemId = e.TaskItemId,
                    WorkDate = e.WorkDate,
                    HoursWorked = e.HoursWorked,
                    Note = e.Note
                }).ToList()
            };
        }

        public void SaveMyTimesheet(string username, SaveTimesheetRequest request)
        {
            var empId = GetEmployeeId(username);
            var timesheet = _repo.GetTimesheetByWeek(empId, request.WeekStartDate);
            
            if (timesheet == null)
            {
                // Create new
                timesheet = new Timesheet
                {
                    EmployeeId = empId,
                    WeekStartDate = request.WeekStartDate,
                    WeekEndDate = request.WeekEndDate,
                    CreatedDate = DateTime.Now,
                    Status = request.IsSubmit ? "Pending" : "Draft",
                    Entries = request.Entries.Select(e => new TimesheetEntry
                    {
                        TaskItemId = e.TaskItemId,
                        WorkDate = e.WorkDate,
                        HoursWorked = e.HoursWorked,
                        Note = e.Note,
                        IsOvertime = e.HoursWorked > 8
                    }).ToList()
                };
                _repo.CreateTimesheet(timesheet);
            }
            else
            {
                // Rule: If it's already Approved or Pending, you might not want to allow changes unless it's a Draft or Rejected. 
                // For demonstration, we allow modifying if it's not Approved.
                if (timesheet.Status == "Approved")
                    throw new Exception("Không thể sửa Timesheet đã Approved.");

                timesheet.Status = request.IsSubmit ? "Pending" : "Draft";
                
                // Clear old entries and add new ones
                _repo.DeleteEntries(timesheet.Entries.ToList());
                
                timesheet.Entries = request.Entries.Select(e => new TimesheetEntry
                {
                    TaskItemId = e.TaskItemId,
                    WorkDate = e.WorkDate,
                    HoursWorked = e.HoursWorked,
                    Note = e.Note,
                    IsOvertime = e.HoursWorked > 8
                }).ToList();
                
                _repo.UpdateTimesheet(timesheet);
            }
        }
    }
}

    
