namespace TimeSheetManager.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
    }
}
