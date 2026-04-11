using MediatR;

namespace Application.Organization.Commands.DeletePosition;

public record DeletePositionCommand(
    Guid PositionId,
    Guid? ReassignToPositionId = null,
    bool DeleteAllUsers = false) : IRequest<bool>;
