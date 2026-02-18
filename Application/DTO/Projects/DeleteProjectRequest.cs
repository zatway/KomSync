using MediatR;

namespace Application.DTO.Projects;

public record DeleteProjectRequest(Guid Id) : IRequest<bool>;