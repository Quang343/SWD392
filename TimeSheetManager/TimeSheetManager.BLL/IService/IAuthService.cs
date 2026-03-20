using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.DTO;

namespace TimeSheetManager.BLL.IService
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
    }
}
