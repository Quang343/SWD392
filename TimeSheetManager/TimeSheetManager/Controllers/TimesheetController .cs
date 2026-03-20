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
    }
}
