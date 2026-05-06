using Application.Common;
using Application.DTO.Search;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Search.Queries.GlobalSearch;

public class GlobalSearchHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GlobalSearchQuery, IReadOnlyList<SearchHitDto>>
{
    public async Task<IReadOnlyList<SearchHitDto>> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var raw = request.Q.Trim();
        if (raw.Length < 2) return Array.Empty<SearchHitDto>();

        var searchText = raw;
        var take = Math.Clamp(request.Take, 1, 100);
        var perKind = Math.Max(1, take / 4);

        IQueryable<Domain.Entities.Project> visible = context.Projects.Where(p => !p.IsArchived);
        visible = visible.WhereUserCanSeeProject(role, uid, currentUser.DepartmentId);

        var visibleIds = await visible.Select(p => p.Id).ToListAsync(cancellationToken);

        var hits = new List<SearchHitDto>();

        var projects = await visible
            .AsNoTracking()
            .Where(p => EF.Functions
                .ToTsVector("simple", (p.Name ?? string.Empty) + " " + (p.Description ?? string.Empty))
                .Matches(searchText))
            .Take(perKind)
            .Select(p => new SearchHitDto("project", p.Id, p.Name, p.Key, p.Id, p.Key))
            .ToListAsync(cancellationToken);
        hits.AddRange(projects);

        if (visibleIds.Count > 0)
        {
            var tasks = await context.Tasks
                .AsNoTracking()
                .Include(t => t.Project)
                .Where(t => visibleIds.Contains(t.ProjectId))
                .Where(t => EF.Functions
                    .ToTsVector("simple", (t.Title ?? string.Empty) + " " + (t.Description ?? string.Empty))
                    .Matches(searchText))
                .Take(perKind)
                .ToListAsync(cancellationToken);

            foreach (var t in tasks)
            {
                hits.Add(new SearchHitDto(
                    "task",
                    t.Id,
                    t.Title,
                    t.Project?.Key,
                    t.ProjectId,
                    t.Project?.Key));
            }

            var taskComments = await context.TaskComments
                .AsNoTracking()
                .Include(c => c.Task)
                .ThenInclude(t => t!.Project)
                .Where(c => c.Task != null && visibleIds.Contains(c.Task.ProjectId))
                .Where(c => EF.Functions
                    .ToTsVector("simple", c.Content ?? string.Empty)
                    .Matches(searchText))
                .Take(perKind)
                .ToListAsync(cancellationToken);

            foreach (var c in taskComments)
            {
                var preview = c.Content.Length > 80 ? c.Content[..80] + "…" : c.Content;
                hits.Add(new SearchHitDto(
                    "taskComment",
                    c.Id,
                    preview,
                    c.Task?.Title,
                    c.Task?.ProjectId,
                    c.Task?.Project?.Key));
            }

            var projectComments = await context.ProjectComments
                .AsNoTracking()
                .Where(c => visibleIds.Contains(c.ProjectId))
                .Where(c => EF.Functions
                    .ToTsVector("simple", c.Content ?? string.Empty)
                    .Matches(searchText))
                .Take(perKind)
                .ToListAsync(cancellationToken);

            foreach (var c in projectComments)
            {
                var preview = c.Content.Length > 80 ? c.Content[..80] + "…" : c.Content;
                hits.Add(new SearchHitDto("projectComment", c.Id, preview, null, c.ProjectId, null));
            }
        }

        var articles = await context.KnowledgeArticles
            .AsNoTracking()
            .Where(a => EF.Functions
                .ToTsVector("simple", (a.Title ?? string.Empty) + " " + (a.ContentMarkdown ?? string.Empty))
                .Matches(searchText))
            .Take(perKind)
            .Select(a => new SearchHitDto("knowledge", a.Id, a.Title, a.Slug, null, null))
            .ToListAsync(cancellationToken);
        hits.AddRange(articles);

        return hits.Take(take).ToList();
    }
}
