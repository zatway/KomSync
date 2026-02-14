using MediatR;

namespace Application.Projects.Commands;

public record CreateProjectCommand(string Name, string? Description, Guid OwnerId) : IRequest<Guid>;
