using Microsoft.EntityFrameworkCore;
using System.Linq;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TimesheetDbContext _context;

        public EmployeeRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public Employee GetByUserId(int userId)
        {
            return _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.UserId == userId);
        }

        public Employee GetByUsername(string username)
        {
            // INCLUDE để có e.User.Username
            return _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Username == username);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
    }
}
