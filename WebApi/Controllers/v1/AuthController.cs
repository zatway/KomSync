using Application.Auth.Commands.Register;
using Application.DTO.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        // Валидация сработает автоматически в ValidationBehavior!
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var response = await authService.RefreshTokenAsync(refreshToken);
        if (response == null) 
            return Unauthorized(new { message = "Невалидный токен обновления" });

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        await authService.RevokeTokenAsync(refreshToken);
        return NoContent();
    }
}