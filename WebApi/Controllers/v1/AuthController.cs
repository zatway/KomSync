using Application.DTO.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IMediator mediator, ICurrentUserService _currentUser) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest command)
    {
        var response = await mediator.Send(command); 
        return Ok(response);
    } 
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var response = await mediator.Send(new MeRequest(_currentUser.UserId)); 
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
        var response = await mediator.Send(new RevokeTokenRequest(_currentUser.UserId)); 
        return Ok(response);
    }
}