using Application.DTO.Projects;
using Application.Interfaces;
using Application.Projects.ProjectTaskStatusColumns;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Projects.Commands.CreateProject
{
    public class CreateProjectHandler(IFmkSyncContext context, IMapper mapper, ICurrentUserService currentUser)
        : IRequestHandler<CreateProjectRequest, Guid>
    {
        public async Task<Guid> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId == null)
                throw new UnauthorizedAccessException("User not authorized");

            var project = mapper.Map<Project>(request);
            project.OwnerId = currentUser.UserId.Value;

            await context.Projects.AddAsync(project, cancellationToken);
            ProjectTaskStatusColumnSeeder.SeedDefaultsForProject(context, project.Id);
            await context.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }
}