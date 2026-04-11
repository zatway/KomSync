using Application.DTO.Auth;
using Application.DTO.UserProfile;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/profile")]
public class UserProfileController(IMediator mediator, ICurrentUserService _currentUser) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var response = await mediator.Send(new MeRequest(_currentUser.UserId)); 
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileRequest request)
    {
        var result = await mediator.Send(request);
        return result ? NoContent() : NotFound();
    }
    
    [Authorize]
    [HttpGet("me/avatar")]
    public async Task<IActionResult> GetAvatar()
    {
        var result = await mediator.Send(new MeAvatarRequest(_currentUser.UserId));

        return File(result.Data, result.ContentType);
    }

    /// <summary>Аватар любого пользователя (для UI списков). Доступно авторизованным клиентам.</summary>
    [Authorize]
    [HttpGet("users/{userId:guid}/avatar")]
    public async Task<IActionResult> GetUserAvatar(Guid userId)
    {
        var result = await mediator.Send(new MeAvatarRequest(userId));
        return File(result.Data, result.ContentType);
    }
}