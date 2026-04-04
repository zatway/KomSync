using Application.Admin.Commands.ApproveRegistration;
using Application.Admin.Commands.RejectRegistration;
using Application.Admin.Commands.UpdateUserAdmin;
using Application.Admin.Commands.UpdateUserRole;
using Application.Admin.Queries.GetAdminUsers;
using Application.Admin.Queries.GetPendingRegistrations;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("registrations")]
    public async Task<IActionResult> GetPendingRegistrations()
    {
        var list = await mediator.Send(new GetPendingRegistrationsQuery());
        return Ok(list);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var list = await mediator.Send(new GetAdminUsersQuery());
        return Ok(list);
    }

    [HttpPost("registrations/{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var ok = await mediator.Send(new ApproveRegistrationCommand(id));
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("registrations/{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var ok = await mediator.Send(new RejectRegistrationCommand(id));
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("users/{userId:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid userId, [FromBody] UpdateUserRoleBody body)
    {
        var ok = await mediator.Send(new UpdateUserRoleCommand(userId, body.Role));
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("users/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserAdminBody body)
    {
        var ok = await mediator.Send(new UpdateUserAdminCommand(
            userId,
            FullName: body.FullName,
            Email: body.Email,
            IsApproved: body.IsApproved,
            Role: body.Role,
            DepartmentId: body.DepartmentId,
            PositionId: body.PositionId
        ));
        return ok ? NoContent() : NotFound();
    }

    public record UpdateUserRoleBody(UserRole Role);

    public record UpdateUserAdminBody(
        string? FullName,
        string? Email,
        bool? IsApproved,
        UserRole? Role,
        Guid? DepartmentId,
        Guid? PositionId);
}
