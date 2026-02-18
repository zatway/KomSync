using MediatR;

namespace Application.DTO.Projects;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDetailedDto?>;