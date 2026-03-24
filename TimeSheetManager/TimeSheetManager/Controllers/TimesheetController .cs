using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheetManager.BLL.IService;

namespace TimeSheetManager.Controllers
{
    [ApiController]
    [Route("api/timesheet")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _service;

        public TimesheetController(ITimesheetService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [Authorize]
        [HttpGet("my-tasks")]
        public IActionResult GetMyTasks()
        {
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized(new { message = "Invalid Token" });

            return Ok(_service.GetMyTasks(username));
        }

        [Authorize]
        [HttpGet("available-tasks")]
        public IActionResult GetAvailableTasks()
        {
            // Cho phép lấy tất cả Task trong hệ thống (mô phỏng)
            return Ok(_service.GetAvailableTasks());
        }

        [Authorize]
        [HttpGet("my-timesheet")]
        public IActionResult GetMyTimesheet([FromQuery] DateTime startDate)
        {
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized(new { message = "Invalid Token" });

            var timesheet = _service.GetMyTimesheet(username, startDate);
            return Ok(timesheet); // returns null 200/204 if none, which is fine.
        }

        [Authorize]
        [HttpPost("save")]
        public IActionResult SaveMyTimesheet([FromBody] TimeSheetManager.BLL.DTO.SaveTimesheetRequest request)
        {
            try
            {
                var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized(new { message = "Invalid Token" });

                _service.SaveMyTimesheet(username, request);
                return Ok(new { message = "Lưu timesheet thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("my-timesheets")]
        public IActionResult GetMyTimesheets()
        {
            try
            {
                var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized(new { message = "Invalid Token" });

                return Ok(_service.GetMyTimesheets(username));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("my-timesheet/{id}")]
        public IActionResult DeleteMyTimesheet(int id)
        {
            try
            {
                var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(username)) return Unauthorized(new { message = "Invalid Token" });

                _service.DeleteMyTimesheet(username, id);
                return Ok(new { message = "Xoá timesheet thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
