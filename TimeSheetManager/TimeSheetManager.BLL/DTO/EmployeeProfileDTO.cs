using System;

namespace TimeSheetManager.BLL.DTO
{
    public class EmployeeProfileDTO
    {
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Status { get; set; }
        public string? Username { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
    }
}
