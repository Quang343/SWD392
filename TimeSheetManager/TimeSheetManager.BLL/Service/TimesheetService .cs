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

        public TimesheetService(ITimesheetRepository repo)
        {
            _repo = repo;
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
    }
}

    
