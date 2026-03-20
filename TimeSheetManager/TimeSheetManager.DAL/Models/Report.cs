namespace TimeSheetManager.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportType { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string Parameters { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
