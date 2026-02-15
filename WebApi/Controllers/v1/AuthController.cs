using Application.DTO.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest command)
    {
        // Валидация сработает автоматически в ValidationBehavior!
        var response = await mediator.Send(command);
        return Ok(response);
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
    public async Task<IActionResult> Logout([FromBody] RevokeTokenRequest refreshToken)
    {
        var response = await mediator.Send(refreshToken); 
        return Ok(response);
    }
}