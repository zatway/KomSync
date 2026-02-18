using Domain.Enums;
using MediatR;

namespace Application.DTO.Tasks;

public record CreateTaskRequest(
    string Title,
    string Description,
    ProjectTaskStatus Status,
    ProjectTaskPriority Priority,
    Guid ProjectId) : IRequest<Guid>;