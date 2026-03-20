namespace TimeSheetManager.Models
{
    public class TimesheetEntry
    {
        public int TimesheetEntryId { get; set; }
        public DateTime WorkDate { get; set; }
        public double HoursWorked { get; set; }
        public bool IsOvertime { get; set; }
        public string Note { get; set; }

        public int TimesheetId { get; set; }
        public Timesheet Timesheet { get; set; }

        public int TaskItemId { get; set; }
        public TaskItem Task { get; set; }
    }
}
