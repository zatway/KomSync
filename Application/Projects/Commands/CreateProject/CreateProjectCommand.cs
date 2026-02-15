using MediatR;

namespace Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(string Name, string? Description, Guid OwnerId) : IRequest<Guid>;
