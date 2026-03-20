namespace TimeSheetManager.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TimeSheetManager.BLL.DTO;
    using TimeSheetManager.BLL.IService;

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var result = _service.Login(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
