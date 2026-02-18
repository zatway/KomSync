using MediatR;

namespace Application.DTO.Projects;

public record UpdateProjectRequest(Guid Id, string? Name, Guid? OwnerId, string? Description) : IRequest<bool>;
