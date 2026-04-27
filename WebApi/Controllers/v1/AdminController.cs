using Application.Admin.Commands.ApproveRegistration;
using Application.Admin.Commands.DeleteUserAdmin;
using Application.Admin.Commands.RejectRegistration;
using Application.Admin.Commands.UpdateUserAdmin;
using Application.Admin.Commands.UpdateUserRole;
using Application.Admin.Queries.GetAdminUsers;
using Application.Admin.Queries.GetPendingRegistrations;
using Application.Interfaces;
using Application.Organization.Queries.GetDepartmentUsers;
using Application.Organization.Queries.GetPositionUsers;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers.v1;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController(IMediator mediator, IFmkSyncContext context) : ControllerBase
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

    [HttpGet("departments/{departmentId:guid}/users")]
    public async Task<IActionResult> GetDepartmentUsers(Guid departmentId, CancellationToken cancellationToken)
    {
        if (!await context.Departments.AnyAsync(d => d.Id == departmentId, cancellationToken))
            return NotFound();
        var list = await mediator.Send(new GetDepartmentUsersQuery(departmentId), cancellationToken);
        return Ok(list);
    }

    [HttpGet("positions/{positionId:guid}/users")]
    public async Task<IActionResult> GetPositionUsers(Guid positionId, CancellationToken cancellationToken)
    {
        if (!await context.Positions.AnyAsync(p => p.Id == positionId, cancellationToken))
            return NotFound();
        var list = await mediator.Send(new GetPositionUsersQuery(positionId), cancellationToken);
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
            PositionId: body.PositionId,
            NewPassword: body.NewPassword
        ));
        return ok ? NoContent() : NotFound();
    } 
    
    [HttpDelete("users/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var ok = await mediator.Send(new DeleteUserAdminCommand(
            userId
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
        Guid? PositionId,
        string? NewPassword);
}
