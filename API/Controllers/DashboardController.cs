using Application.CQRS;
using Application.DTOs;

using Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaaS_Tenant_Manager.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DashboardController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    [Route("GetTenantSubscription")]
    public async Task<IActionResult> GetTenantSubscription([FromBody] Guid TenantId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new GetTenantSubscriptionQuery(TenantId));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }    
    [Authorize(Roles = "SystemAdmin")] 
    [HttpPatch("toggleFeature")]
    public async Task<IActionResult> ToggleFeature([FromBody] ToggleFeatureAccessDTO request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new ToggleFeatureCommand(request));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
    
}


