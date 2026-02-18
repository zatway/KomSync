using Domain.Enums;
using MediatR;

namespace Application.DTO.Tasks;

public record UpdateTaskRequest(
    Guid Id,
    string? Title,
    ProjectTaskStatus? Status,
    ProjectTaskPriority? Priority,
    DateTime? Deadline,
    string? Description,
    Guid ProjectId,
    Guid? ParentTaskId,
    Guid? AssigneeId 
) : IRequest<bool>;