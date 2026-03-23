using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.BLL.IService;
using TimeSheetManager.DAL.Repository;
using TimeSheetManager.Models;

namespace TimeSheetManager.BLL.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public EmployeeProfileDTO GetProfile(string username)
        {
            var emp = _repo.GetByUsername(username);
            if (emp == null) throw new Exception("Employee not found");

            return new EmployeeProfileDTO
            {
                EmployeeId = emp.EmployeeId,
                FullName = emp.FullName,
                Email = emp.Email,
                Department = emp.Department,
                Position = emp.Position,
                Status = emp.Status,
                Username = emp.User?.Username
            };
        }

        public void UpdateProfile(string username, UpdateProfileRequest request)
        {
            var emp = _repo.GetByUsername(username);
            if (emp == null) throw new Exception("Employee not found");

            // Update allowed fields
            emp.FullName = request.FullName;
            emp.Department = request.Department;
            emp.Position = request.Position;

            _repo.Update(emp);
        }
    }
}
