using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetManager.BLL.DTO
{
    public class TimesheetDTO
    {
        public int TimesheetId { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; }
        public List<TimesheetEntryDTO> Entries { get; set; }
    }
    public class TimesheetEntryDTO
    {
        public DateTime WorkDate { get; set; }
        public double HoursWorked { get; set; }
        public string TaskName { get; set; }
    }
}
