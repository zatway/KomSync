using MediatR;

namespace Application.DTO.Projects;

public record UpdateProjectRequest(
    Guid Id,
    string? Name,
    string? Key,
    string? Description,
    DateTime? StartDate,
    DateTime? DueDate,
    string? Color,
    string? Icon,
    List<string>? Tags,
    bool? IsArchived,
    Guid? DepartmentId
) : IRequest<bool>;