using Application.CQRS;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace SaaS_Tenant_Manager.Controllers;
[Authorize(Roles = "TenantAdmin,TenantUser")]
[Route("api/[controller]")]
[ApiController]
public class ShipmentsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("CreateShipment")]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new CreateShipmentCommand(dto));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
    [HttpPut]
    [Route("UpdateShipmentStatus")]
    public async Task<IActionResult> UpdateShipmentStatus([FromBody] UpdateShipmentDTO DTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await mediator.Send(new UpdateShipmentStatusCommand(DTO));
        var response = ApiResponse.Success(result);

        return Ok(response);
    }
    [HttpGet]
    [Route("GetTenantShipments")]
    public async Task<IActionResult> GetTenantShipments()
    {
        var result = await mediator.Send(new GetTenantShipmentsQuery());
        var response = ApiResponse.Success(result);

        return Ok(response);
    }

}
