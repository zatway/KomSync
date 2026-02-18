using MediatR;

namespace Application.DTO.Tasks;

public record AssignUserRequest(Guid TaskId, Guid? AssigneeId) : IRequest<bool>;