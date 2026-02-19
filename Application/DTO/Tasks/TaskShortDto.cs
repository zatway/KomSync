using Application.DTO.TaskComment;
using Application.DTO.TaskHistory;
using Domain.Enums;

namespace Application.DTO.Tasks;

public record TaskShortDto
{
    public TaskShortDto()
    {
    }

    public Guid Id { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public ProjectTaskPriority Priority { get; init; }
    public Guid CreatorId {get; set;}
    public Guid? AssigneeId {get; set;}
}