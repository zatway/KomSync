using MediatR;

namespace Application.DTO.Projects;

public record GetProjectsQuery(bool IncludeArchived = false) : IRequest<List<ProjectBriefDto>>;
