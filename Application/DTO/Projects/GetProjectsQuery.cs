using MediatR;

namespace Application.DTO.Projects;

public record GetProjectsQuery() : IRequest<List<ProjectBriefDto>>;
