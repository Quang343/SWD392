using Microsoft.EntityFrameworkCore;
using TimeSheetManager.Models;

public class TimesheetDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
    public DbSet<Timesheet> Timesheets { get; set; }
    public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
    public DbSet<Approval> Approvals { get; set; }
    public DbSet<Report> Reports { get; set; }

    public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========================
        // ROLE - USER (1 - n)
        // ========================
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // ========================
        // USER - EMPLOYEE (1 - 1)
        // ========================
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithOne(u => u.Employee)
            .HasForeignKey<Employee>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.UserId)
            .IsUnique(); // đảm bảo 1-1

        // ========================
        // EMPLOYEE - TIMESHEET (1 - n)
        // ========================
        modelBuilder.Entity<Timesheet>()
            .HasOne(t => t.Employee)
            .WithMany(e => e.Timesheets)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // TIMESHEET - ENTRY (1 - n)
        // ========================
        modelBuilder.Entity<TimesheetEntry>()
            .HasOne(te => te.Timesheet)
            .WithMany(t => t.Entries)
            .HasForeignKey(te => te.TimesheetId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // PROJECT - TASK (1 - n)
        // ========================
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // TASK - TIMESHEET ENTRY (1 - n)
        // ========================
        modelBuilder.Entity<TimesheetEntry>()
            .HasOne(te => te.Task)
            .WithMany(t => t.TimesheetEntries)
            .HasForeignKey(te => te.TaskItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // ========================
        // EMPLOYEE - PROJECT (n - n via ProjectAssignment)
        // ========================
        modelBuilder.Entity<ProjectAssignment>()
            .HasOne(pa => pa.Employee)
            .WithMany(e => e.ProjectAssignments)
            .HasForeignKey(pa => pa.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectAssignment>()
            .HasOne(pa => pa.Project)
            .WithMany(p => p.ProjectAssignments)
            .HasForeignKey(pa => pa.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // TIMESHEET - APPROVAL (1 - n)
        // ========================
        modelBuilder.Entity<Approval>()
            .HasOne(a => a.Timesheet)
            .WithMany(t => t.Approvals)
            .HasForeignKey(a => a.TimesheetId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // EMPLOYEE - REPORT (1 - n)
        // ========================
        modelBuilder.Entity<Report>()
            .HasOne(r => r.Employee)
            .WithMany(e => e.Reports)
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // ========================
        // OPTIONAL: CONFIG FIELD
        // ========================
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Email)
            .HasMaxLength(150);

        modelBuilder.Entity<Project>()
            .Property(p => p.ProjectName)
            .IsRequired();

        modelBuilder.Entity<TaskItem>()
            .Property(t => t.TaskName)
            .IsRequired();
        //APPROVAL - APPROVEDBYUSER (n - 1)
        modelBuilder.Entity<Approval>()
    .HasOne(a => a.ApprovedByUser)
    .WithMany()
    .HasForeignKey(a => a.ApprovedByUserId)
    .OnDelete(DeleteBehavior.Restrict);
    }
}