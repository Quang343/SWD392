namespace TimeSheetManager.Models
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string TaskName { get; set; }
        public double EstimatedHours { get; set; }
        public string Status { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }
    }
}
