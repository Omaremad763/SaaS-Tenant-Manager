
using Application.CQRS;
using Application.DTOs;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaaS_Tenant_Manager.Controllers;
[Authorize(Roles = "TenantAdmin,TenantUser")]
[ApiController]
[Route("api/[controller]")]
public class ClientsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("CreateClient")]
    public async Task<IActionResult> CreateClient([FromBody] ClientDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new CreateClientCommand(dto));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("UpdateClient")]
    public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new UpdateClientCommand(dto));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }

    [HttpDelete]
    [Route("DeleteClient/{id:guid}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid client id.");
        var result = await mediator.Send(new DeleteClientCommand(id));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }

    [HttpGet]
    [Route("GetAllClients")]
    public async Task<IActionResult> GetAllClients()
    {
        var result = await mediator.Send(new GetAllClientsQuery());
        var response = ApiResponse.Success(result);
        return Ok(response);
    }

    [HttpGet]
    [Route("GetClientById/{id:guid}")]
    public async Task<IActionResult> GetClientById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid client id.");

        var result = await mediator.Send(new GetClientByIdQuery(id));
        var response = ApiResponse.Success(result);

        return Ok(response);
    }
}
