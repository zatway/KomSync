using Application.Common;
using Application.DTO.Analytics;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Analytics.Queries.GetAnalyticsDashboard;

public class GetAnalyticsDashboardHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetAnalyticsDashboardQuery, AnalyticsDashboardDto>
{
    public async Task<AnalyticsDashboardDto> Handle(
        GetAnalyticsDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        IQueryable<Domain.Entities.Project> visible = context.Projects.Where(p => !p.IsArchived);
        if (!ProjectAccessRules.CanViewAllProjects(role))
            visible = visible.Where(p => p.OwnerId == uid || p.Members.Any(m => m.Id == uid));

        var visibleIds = await visible.Select(p => p.Id).ToListAsync(cancellationToken);
        if (visibleIds.Count == 0)
        {
            return new AnalyticsDashboardDto(0, 0, Array.Empty<StatusCountDto>(), Array.Empty<UserLoadDto>());
        }

        var today = DateTime.UtcNow.Date;

        var openTasks = await context.Tasks
            .AsNoTracking()
            .Where(t => visibleIds.Contains(t.ProjectId) && !t.StatusColumn.IsDoneColumn)
            .Include(t => t.StatusColumn)
            .Include(t => t.Assignee)
            .ToListAsync(cancellationToken);

        var openCount = openTasks.Count;
        var overdueCount = openTasks.Count(t => t.Deadline != null && t.Deadline.Value.Date < today);

        var byStatus = openTasks
            .GroupBy(t => t.StatusColumn.Name)
            .Select(g => new StatusCountDto(g.Key, g.Count()))
            .ToList();

        var topAssignees = openTasks
            .Where(t => t.AssigneeId != null && t.Assignee != null)
            .GroupBy(t => new { t.AssigneeId, t.Assignee!.FullName })
            .Select(g => new UserLoadDto(g.Key.AssigneeId!.Value, g.Key.FullName, g.Count()))
            .OrderByDescending(x => x.ActiveTaskCount)
            .Take(10)
            .ToList();

        return new AnalyticsDashboardDto(openCount, overdueCount, byStatus, topAssignees);
    }
}
