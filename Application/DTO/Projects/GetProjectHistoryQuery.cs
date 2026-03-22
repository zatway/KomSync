using MediatR;

namespace Application.DTO.Projects;

public record GetProjectHistoryQuery(Guid ProjectId) : IRequest<List<ProjectHistoryEntryDto>>;
