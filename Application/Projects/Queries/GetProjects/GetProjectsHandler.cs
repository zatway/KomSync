using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjects;

public class GetProjectsHandler(IKomSyncContext context)
    : IRequestHandler<GetProjectsQuery, List<ProjectBriefDto>>
{
    public async Task<List<ProjectBriefDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
            .Include(p => p.Tasks).ThenInclude(t => t.StatusColumn)
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
            p.Tasks.Count(t => t.StatusColumn is { IsDoneColumn: false }),
            p.Tasks.Count(t => t.StatusColumn is { IsDoneColumn: true }),
            p.Progress,
            p.DueDate,
            p.UpdatedAt,
            p.CreatedAt,
            p.UpdatedAt,
            p.Color,
            p.Icon?.ToString()
        )).ToList();
    }
}
