using System;
using System.Collections.Generic;

namespace TimeSheetManager.BLL.DTO
{
    public class MyTaskDTO
    {
        public int TaskItemId { get; set; }
        public string TaskName { get; set; }
        public string ProjectName { get; set; }
    }

    public class MyTimesheetDTO
    {
        public int TimesheetId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string Status { get; set; }
        public List<MyTimesheetEntryDTO> Entries { get; set; } = new List<MyTimesheetEntryDTO>();
    }

    public class MyTimesheetEntryDTO
    {
        public int TimesheetEntryId { get; set; }
        public int TaskItemId { get; set; }
        public DateTime WorkDate { get; set; }
        public double HoursWorked { get; set; }
        public string Note { get; set; }
    }

    public class SaveTimesheetRequest
    {
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public bool IsSubmit { get; set; } // Nếu true -> Submit for Approval (Status = Pending), Nếu false -> Save Draft (Status = Draft)
        public List<SaveTimesheetEntryDTO> Entries { get; set; } = new List<SaveTimesheetEntryDTO>();
    }

    public class SaveTimesheetEntryDTO
    {
        public int TaskItemId { get; set; }
        public DateTime WorkDate { get; set; }
        public double HoursWorked { get; set; }
        public string Note { get; set; }
    }
}
