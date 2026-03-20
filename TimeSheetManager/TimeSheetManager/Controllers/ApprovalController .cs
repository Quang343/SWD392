using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeSheetManager.BLL.DTO;
using TimeSheetManager.BLL.IService;

[ApiController]
[Route("api/approval")]
public class ApprovalController : ControllerBase
{
    private readonly IApprovalService _service;

    public ApprovalController(IApprovalService service)
    {
        _service = service;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst("UserId").Value);
    }

    private string GetRole()
    {
        return User.FindFirst(ClaimTypes.Role).Value;
    }

    [Authorize]
    [HttpPost("approve")]
    public IActionResult Approve(ApprovalRequest request)
    {
        if (GetRole() != "2") // 2 = Manager
            return Forbid("Only Manager can approve");

        _service.Approve(request.TimesheetId, GetUserId(), request.Comment);

        return Ok("Approved");
    }

    [Authorize]
    [HttpPost("reject")]
    public IActionResult Reject(ApprovalRequest request)
    {
        if (GetRole() != "2")
            return Forbid("Only Manager can reject");

        _service.Reject(request.TimesheetId, GetUserId(), request.Comment);

        return Ok("Rejected");
    }
}