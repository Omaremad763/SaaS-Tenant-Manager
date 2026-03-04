using Application.CQRS;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaaS_Tenant_Manager.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class AdminPanelController(IMediator mediator):ControllerBase
{

    [HttpGet]
    [Authorize(Roles = "SystemAdmin")]
    [Route("GetSystemMetrics")]
    public async Task<IActionResult> GetSystemMetrics()
    {
        var result = await mediator.Send(new GetApiLogQuery());
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
}