using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Knowledge;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Knowledge.Queries.GetKnowledgeArticles;

public class GetKnowledgeArticlesHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetKnowledgeArticlesQuery, IReadOnlyList<KnowledgeArticleListItemDto>>
{
    public async Task<IReadOnlyList<KnowledgeArticleListItemDto>> Handle(
        GetKnowledgeArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        IQueryable<Domain.Entities.KnowledgeArticle> query = context.KnowledgeArticles.AsNoTracking();

        if (request.TaskId.HasValue)
        {
            var task = await context.Tasks
                .Include(t => t.Project)
                .ThenInclude(p => p.Members)
                .FirstOrDefaultAsync(t => t.Id == request.TaskId.Value, cancellationToken)
                ?? throw new NotFoundException("Задача не найдена");

            if (!ProjectAccessRules.UserCanViewProject(role, uid, task.Project, currentUser.DepartmentId))
                throw new ForbiddenException("Нет доступа к задаче");

            query = query.Where(a => a.ProjectTaskId == request.TaskId.Value);
        }
        else if (request.ProjectId.HasValue)
        {
            var project = await context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId.Value, cancellationToken)
                ?? throw new NotFoundException("Проект не найден");

            if (!ProjectAccessRules.UserCanViewProject(role, uid, project, currentUser.DepartmentId))
                throw new ForbiddenException("Нет доступа к проекту");

            var pid = request.ProjectId.Value;
            query = query.Where(a =>
                a.ProjectId == pid
                || (a.ProjectTaskId != null &&
                    context.Tasks.Any(t => t.Id == a.ProjectTaskId && t.ProjectId == pid)));
        }
        else if (!ProjectAccessRules.CanViewAllProjects(role))
        {
            var accessibleIds = context.Projects
                .Where(p => !p.IsArchived)
                .WhereUserCanSeeProject(role, uid, currentUser.DepartmentId)
                .Select(p => p.Id);

            query = query.Where(a =>
                (a.ProjectId == null && a.ProjectTaskId == null)
                || (a.ProjectId != null && accessibleIds.Contains(a.ProjectId.Value))
                || (a.ProjectTaskId != null &&
                    context.Tasks.Any(t => t.Id == a.ProjectTaskId && accessibleIds.Contains(t.ProjectId))));
        }

        var rows = await query
            .Include(a => a.Project)
            .Include(a => a.LinkedTask)
            .ThenInclude(t => t!.Project)
            .OrderBy(a => a.ParentId)
            .ThenBy(a => a.SortOrder)
            .ThenBy(a => a.Title)
            .ToListAsync(cancellationToken);

        return rows.Select(MapList).ToList();
    }

    private static KnowledgeArticleListItemDto MapList(Domain.Entities.KnowledgeArticle a)
    {
        string? taskKey = null;
        if (a.LinkedTask != null && a.Project != null)
            taskKey = $"{a.Project.Key}-{a.LinkedTask.TaskNumber}";
        else if (a.LinkedTask?.Project != null)
            taskKey = $"{a.LinkedTask.Project.Key}-{a.LinkedTask.TaskNumber}";

        return new KnowledgeArticleListItemDto(
            a.Id,
            a.Title,
            a.Slug,
            a.ParentId,
            a.SortOrder,
            a.UpdatedAt,
            a.ProjectId,
            a.Project?.Key,
            a.Project?.Name,
            a.ProjectTaskId,
            taskKey);
    }
}
