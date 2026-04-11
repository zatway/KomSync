using Application.Common;
using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjects;

public class GetProjectsHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetProjectsQuery, List<ProjectBriefDto>>
{
    public async Task<List<ProjectBriefDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        IQueryable<Domain.Entities.Project> query = context.Projects;

        if (!request.IncludeArchived)
            query = query.Where(p => !p.IsArchived);

        if (!ProjectAccessRules.CanViewAllProjects(role))
            query = query.Where(p => p.OwnerId == userId || p.Members.Any(m => m.Id == userId));

        var projects = await query
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
            ProjectProgressCalculator.Compute(p.Tasks.ToList()),
            p.DueDate,
            p.UpdatedAt,
            p.CreatedAt,
            p.UpdatedAt,
            p.Color,
            p.Icon?.ToString(),
            p.IsArchived
        )).ToList();
    }
}
