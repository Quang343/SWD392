using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.DTO;

namespace TimeSheetManager.BLL.IService
{
    public interface IEmployeeService
    {
        EmployeeProfileDTO GetProfile(string username);
        void UpdateProfile(string username, UpdateProfileRequest request);
    }
}
