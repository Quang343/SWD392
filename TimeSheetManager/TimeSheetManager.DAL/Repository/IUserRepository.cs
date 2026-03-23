using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        bool ExistsByUsername(string username);
        void CreateUser(User user);
    }
}
