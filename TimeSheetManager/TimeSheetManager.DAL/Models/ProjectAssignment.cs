namespace TimeSheetManager.Models
{
    public class ProjectAssignment
    {
        public int ProjectAssignmentId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
