namespace TimeSheetManager.Models
{
    public class Approval
    {
        public int ApprovalId { get; set; }

        public DateTime ApprovalDate { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public int? ApprovedByUserId { get; set; }

        public User ApprovedByUser { get; set; }

        public int TimesheetId { get; set; }
        public Timesheet Timesheet { get; set; }
    }
}
