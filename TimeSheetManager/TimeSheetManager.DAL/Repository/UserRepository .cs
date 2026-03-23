using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TimesheetDbContext _context;

        public UserRepository(TimesheetDbContext context)
        {
            _context = context;
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .Where(u => u.Username == username && u.IsActive)
                .FirstOrDefault();
        }

        public bool ExistsByUsername(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
