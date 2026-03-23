

namespace TimeSheetManager.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Timesheet> Timesheets { get; set; }
        public ICollection<ProjectAssignment> ProjectAssignments { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
