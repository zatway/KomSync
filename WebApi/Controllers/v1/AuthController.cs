using Application.DTO.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator, ICurrentUserService _currentUser) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest command)
    {
        var response = await mediator.Send(command); 
        return Ok(response);
    } 

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshToken)
    {
        var response = await mediator.Send(refreshToken); 
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new RevokeTokenRequest(_currentUser.UserId));
        return NoContent();
    }
}