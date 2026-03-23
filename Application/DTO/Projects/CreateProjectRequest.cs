using MediatR;

namespace Application.DTO.Projects;

public record CreateProjectRequest(
    string Name,
    string Key,
    string? Description,
    DateTime? StartDate,
    DateTime? DueDate,
    string? Color,
    string? Icon,
    Guid DepartmentId,
    List<string>? Tags
) : IRequest<Guid>;
