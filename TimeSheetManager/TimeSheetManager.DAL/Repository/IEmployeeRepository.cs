using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.Models;

namespace TimeSheetManager.DAL.Repository
{
    public interface IEmployeeRepository
    {
        Employee GetByUserId(int userId);
        Employee GetByUsername(string username);
        void Update(Employee employee);
    }
}
