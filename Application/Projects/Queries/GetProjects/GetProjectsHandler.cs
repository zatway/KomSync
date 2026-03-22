using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjects
{
    public class GetProjectsHandler(IKomSyncContext context)
        : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectBriefDto>>
    {
        public async Task<IEnumerable<ProjectBriefDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                .ToListAsync(cancellationToken);

            return projects.Select(p => new ProjectBriefDto(
                p.Id,
                p.Key,
                p.Name,
                p.Description,
                p.OwnerId,
                p.Owner.FullName,
                p.Members.Count,
                p.Tasks.Count,
                p.Tasks.Count(t => t.Status != ProjectTaskStatus.Done),
                p.Tasks.Count(t =>  t.Status == ProjectTaskStatus.Done),
                p.Progress,
                p.DueDate,
                p.UpdatedAt,
                p.CreatedAt,
                p.UpdatedAt,
                p.Color,
                p.Icon?.ToString()
            ));
        }
    }
}