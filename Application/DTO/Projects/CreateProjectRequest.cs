using MediatR;

namespace Application.DTO.Projects;

public record CreateProjectRequest(string Name, Guid OwnerId) : IRequest<Guid>;