using MediatR;

namespace Application.Organization.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    Guid DepartmentId,
    Guid? ReassignToDepartmentId = null,
    Guid? PositionIdForReassignedUsers = null,
    bool DeleteAllUsers = false) : IRequest<bool>;
