using Domain.Enums;
using MediatR;

namespace Application.Admin.Commands.UpdateUserAdmin;

public record UpdateUserAdminCommand(
    Guid UserId,
    string? FullName = null,
    string? Email = null,
    bool? IsApproved = null,
    UserRole? Role = null,
    Guid? DepartmentId = null,
    Guid? PositionId = null,
    string? NewPassword = null
) : IRequest<bool>;

