using MediatR;

namespace Application.DTO.Projects;

public record GetProjectsListQuery : IRequest<List<ProjectBriefDto>>;
