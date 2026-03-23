using Org.BouncyCastle.Crypto.Generators;
using TimeSheetManager.Models;

namespace TimeSheetManager
{
    public class SeedData
    {
        public static void Initialize(TimesheetDbContext context)
        {
         
            // ========================
            // ROLE
            // ========================
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin", Description = "System Admin" },
                    new Role { RoleName = "Manager", Description = "Project Manager" },
                    new Role { RoleName = "HR", Description = "Human Resources" },
                    new Role { RoleName = "Employee", Description = "Employee" }
                );
                context.SaveChanges();
            }

            // ========================
            // USER
            // ========================
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 1 },
                    new User { Username = "manager", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 2 },
                    new User { Username = "hr1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 3 },
                    new User { Username = "emp1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 4 },
                    new User { Username = "emp2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 4 },
                    new User { Username = "emp3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"), IsActive = true, RoleId = 4 }
                );
                context.SaveChanges();
            }

            // ========================
            // EMPLOYEE
            // ========================
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee { FullName = "Admin", Email = "admin@mail.com", Department = "IT", Position = "Admin", Status = "Active", UserId = 1 },
                    new Employee { FullName = "Manager", Email = "manager@mail.com", Department = "IT", Position = "Manager", Status = "Active", UserId = 2 },
                    new Employee { FullName = "Dev One", Email = "emp1@mail.com", Department = "Dev", Position = "Backend", Status = "Active", UserId = 4 },
                    new Employee { FullName = "HR Staff",Email = "hr@mail.com",Department = "HR",Position = "HR Manager",Status = "Active",UserId = 3 },
                    new Employee { FullName = "Tester", Email = "emp2@mail.com", Department = "QA", Position = "Tester", Status = "Active", UserId = 5 },
                    new Employee { FullName = "Frontend Dev", Email = "emp3@mail.com", Department = "Dev", Position = "Frontend", Status = "Active", UserId = 6 }
                );
                context.SaveChanges();
            }

            // ========================
            // PROJECT
            // ========================
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new Project { ProjectName = "Timesheet System", CustomerName = "Internal", StartDate = DateTime.Now, Status = "Active" },
                    new Project { ProjectName = "Ecommerce Website", CustomerName = "Client A", StartDate = DateTime.Now, Status = "Active" }
                );
                context.SaveChanges();
            }

            // ========================
            // TASK
            // ========================
            if (!context.Tasks.Any())
            {
                context.Tasks.AddRange(
                    new TaskItem { TaskName = "API Development", EstimatedHours = 100, Status = "Open", ProjectId = 1 },
                    new TaskItem { TaskName = "UI Design", EstimatedHours = 80, Status = "Open", ProjectId = 1 },
                    new TaskItem { TaskName = "Testing", EstimatedHours = 60, Status = "Open", ProjectId = 2 }
                );
                context.SaveChanges();
            }
        
            // ========================
            // PROJECT ASSIGNMENT
            // ========================
            if (!context.ProjectAssignments.Any())
            {
                context.ProjectAssignments.AddRange(
                    new ProjectAssignment { EmployeeId = 3, ProjectId = 1, AssignedDate = DateTime.Now, Status = "Active" },
                    new ProjectAssignment { EmployeeId = 5, ProjectId = 1, AssignedDate = DateTime.Now, Status = "Active" },
                    new ProjectAssignment { EmployeeId = 3, ProjectId = 2, AssignedDate = DateTime.Now, Status = "Active" },
                    new ProjectAssignment { EmployeeId = 6, ProjectId = 2, AssignedDate = DateTime.Now, Status = "Active" }
                );
                context.SaveChanges();
            }





            // ========================
            // TIMESHEET
            // ========================
            if (!context.Timesheets.Any())
            {
                context.Timesheets.AddRange(
                    new Timesheet { EmployeeId = 6, WeekStartDate = new DateTime(2025, 3, 10), WeekEndDate = new DateTime(2025, 3, 16), CreatedDate = DateTime.Now, Status = "Approved" },
                    new Timesheet { EmployeeId = 6, WeekStartDate = new DateTime(2025, 3, 17), WeekEndDate = new DateTime(2025, 3, 23), CreatedDate = DateTime.Now, Status = "Pending" },
                    new Timesheet { EmployeeId = 3, WeekStartDate = new DateTime(2025, 3, 17), WeekEndDate = new DateTime(2025, 3, 23), CreatedDate = DateTime.Now, Status = "Rejected" },
                    new Timesheet { EmployeeId = 5, WeekStartDate = new DateTime(2025, 3, 17), WeekEndDate = new DateTime(2025, 3, 23), CreatedDate = DateTime.Now, Status = "Pending" }
                );
                context.SaveChanges();
            }

            // ========================
            // TIMESHEET ENTRY
            // ========================
            if (!context.TimesheetEntries.Any())
            {
                context.TimesheetEntries.AddRange(

                    // Employee 6
                    new TimesheetEntry { TimesheetId = 1, TaskItemId = 1, WorkDate = new DateTime(2025, 3, 10), HoursWorked = 8, IsOvertime = false, Note = "API" },
                    new TimesheetEntry { TimesheetId = 1, TaskItemId = 1, WorkDate = new DateTime(2025, 3, 11), HoursWorked = 9, IsOvertime = true, Note = "OT Deploy" },

                    new TimesheetEntry { TimesheetId = 2, TaskItemId = 2, WorkDate = new DateTime(2025, 3, 17), HoursWorked = 7.5, IsOvertime = false, Note = "UI" },

                    // Employee 3 (was 4 in seed)
                    new TimesheetEntry { TimesheetId = 3, TaskItemId = 3, WorkDate = new DateTime(2025, 3, 17), HoursWorked = 6, IsOvertime = false, Note = "Testing" },

                    // Employee 5
                    new TimesheetEntry { TimesheetId = 4, TaskItemId = 2, WorkDate = new DateTime(2025, 3, 18), HoursWorked = 8, IsOvertime = false, Note = "Frontend" }
                );

                context.SaveChanges();
            }

            // ========================
            // APPROVAL
            // ========================
            if (!context.Approvals.Any())
            {
                context.Approvals.AddRange(
                new Approval { TimesheetId = 1, ApprovalDate = DateTime.Now, Status = "Approved", Comment = "Good job" , ApprovedByUserId = 4},
                new Approval { TimesheetId = 2, ApprovalDate = DateTime.Now, Status = "Pending", Comment = "Waiting approval"  ,ApprovedByUserId = null },
                new Approval { TimesheetId = 3, ApprovalDate = DateTime.Now, Status = "Rejected", Comment = "Not enough hours" , ApprovedByUserId = 4},
                new Approval { TimesheetId = 4, ApprovalDate = DateTime.Now, Status = "Pending", Comment = "Waiting review" , ApprovedByUserId = null }
);

                context.SaveChanges();
            }

            // ========================
            // REPORT
            // ========================
            if (!context.Reports.Any())
            {
                context.Reports.AddRange(
            new Report
            {
                EmployeeId = 2,
                ReportType = "Weekly",
                GeneratedDate = DateTime.Now,
                Parameters = "Week=2025-03-17"
            },
    new Report
    {
        EmployeeId = 1,
        ReportType = "Monthly",
        GeneratedDate = DateTime.Now,
        Parameters = "Month=03-2025"
    }
);

                context.SaveChanges();
            }
        }
    }
}
