using Domain.Enums;
using MediatR;

namespace Application.DTO.Tasks;

public record ChangeTaskStatusCommand(Guid TaskId, ProjectTaskStatus NewStatus) : IRequest<bool>;