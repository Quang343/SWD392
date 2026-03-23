using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.Models;

namespace TimeSheetManager.BLL.IService
{
    public interface ITimesheetService
    {
        List<TimesheetDTO> GetAll();
        List<MyTaskDTO> GetMyTasks(string username);
        List<MyTaskDTO> GetAvailableTasks();
        MyTimesheetDTO GetMyTimesheet(string username, DateTime startDate);
        void SaveMyTimesheet(string username, SaveTimesheetRequest request);
    }
}
