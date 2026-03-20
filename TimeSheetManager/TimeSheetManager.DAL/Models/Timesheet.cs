namespace TimeSheetManager.Models
{
    public class Timesheet
    {
        public int TimesheetId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public ICollection<TimesheetEntry> Entries { get; set; }
        public ICollection<Approval> Approvals { get; set; }
    }
}
