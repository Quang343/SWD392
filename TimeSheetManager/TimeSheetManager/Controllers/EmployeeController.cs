using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.BLL.IService;
using System;

namespace TimeSheetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value 
                            ?? User.Identity?.Name;
                
                if (string.IsNullOrEmpty(username)) 
                    return Unauthorized(new { message = "Lỗi xác thực: Không tìm thấy Token hợp lệ." });

                var profile = _employeeService.GetProfile(username);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value 
                            ?? User.Identity?.Name;
                
                if (string.IsNullOrEmpty(username)) 
                    return Unauthorized(new { message = "Lỗi xác thực: Không tìm thấy Token hợp lệ." });

                _employeeService.UpdateProfile(username, request);
                return Ok(new { message = "Cập nhật hồ sơ thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
