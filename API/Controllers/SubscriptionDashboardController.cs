using Application.CQRS;
using Application.DTOs;

using Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace SaaS_Tenant_Manager.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class SubscriptionDashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "TenantAdmin")]
    [Route("GetTenantSubscriptionByTenantId")]
    public async Task<IActionResult> GetTenantSubscription([FromQuery] Guid TenantId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new GetTenantSubscriptionQuery(TenantId));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "SystemAdmin")]
    [Route("GetTenantSubscriptionData")]
    public async Task<IActionResult> GetTenantSubscriptionData()
    {
        var result = await mediator.Send(new GetAllTenantDataQuery());
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
    [Authorize(Roles = "SystemAdmin")] 
    [HttpPatch("toggleFeature")]
    public async Task<IActionResult> ToggleFeature([FromBody] ToggleFeatureAccessDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new ToggleFeatureCommand(request));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
    [HttpGet]
    [Authorize(Roles = "TenantAdmin")]
    [Route("GetClientStatistics")]
    public async Task<IActionResult> GetClientStatistics()
    {
        var result = await mediator.Send(new GetClientStatisticsQuery());
        var response = ApiResponse.Success(result);

        return Ok(response);
    }
}


