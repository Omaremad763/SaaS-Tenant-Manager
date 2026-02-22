using Application.CQRS;
using Application.DTOs;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalResponse.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Route("RegisterTenant")]
        public async Task<IActionResult> RegisterTenant([FromBody] TenantRegistrationDto registrationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await mediator.Send(new RegisterTenantAdminCommand(registrationDto));
            var response = ApiResponse.Success(result);
            return Ok(response);
        }

    [HttpPost]
    [Route("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] TenantUserRegistraionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new RegisterTenantUserCommand(dto));
        var response = ApiResponse.Success(result);
        return Ok(response);

    }
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await mediator.Send(new LoginCommand(dto));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }
}

