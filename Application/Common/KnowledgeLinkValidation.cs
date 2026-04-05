using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public static class KnowledgeLinkValidation
{
    /// <summary>Нормализует связи проект/задача и проверяет доступ.</summary>
    public static async Task<(Guid? ProjectId, Guid? TaskId)> NormalizeAndValidateAsync(
        IKomSyncContext context,
        ICurrentUserService currentUser,
        Guid? requestedProjectId,
        Guid? requestedTaskId,
        CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        if (requestedTaskId.HasValue)
        {
            var task = await context.Tasks
                .Include(t => t.Project)
                .ThenInclude(p => p.Members)
                .FirstOrDefaultAsync(t => t.Id == requestedTaskId.Value, cancellationToken)
                ?? throw new BadRequestException("Задача не найдена");

            if (!ProjectAccessRules.UserCanViewProject(role, uid, task.Project))
                throw new ForbiddenException("Нет доступа к задаче");

            if (requestedProjectId.HasValue && requestedProjectId.Value != task.ProjectId)
                throw new BadRequestException("Задача не относится к указанному проекту");

            return (task.ProjectId, requestedTaskId);
        }

        if (requestedProjectId.HasValue)
        {
            var project = await context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == requestedProjectId.Value, cancellationToken)
                ?? throw new NotFoundException("Проект не найден");

            if (!ProjectAccessRules.UserCanViewProject(role, uid, project))
                throw new ForbiddenException("Нет доступа к проекту");

            return (requestedProjectId, null);
        }

        return (null, null);
    }

    public static async Task ValidateParentScopeAsync(
        IKomSyncContext context,
        Guid? parentId,
        Guid? projectId,
        Guid? taskId,
        CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return;

        var parent = await context.KnowledgeArticles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == parentId.Value, cancellationToken)
            ?? throw new BadRequestException("Родительская статья не найдена");

        if (parent.ProjectId != projectId || parent.ProjectTaskId != taskId)
            throw new BadRequestException(
                "Вложенная статья должна быть в той же области, что и родитель (тот же проект/задача).");
    }

    public static async Task EnsureArticleVisibleAsync(
        IKomSyncContext context,
        ICurrentUserService currentUser,
        KnowledgeArticle article,
        CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        if (ProjectAccessRules.CanViewAllProjects(role))
            return;

        if (article.ProjectId == null && article.ProjectTaskId == null)
            return;

        if (article.ProjectId.HasValue)
        {
            var p = await context.Projects
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == article.ProjectId.Value, cancellationToken);
            if (p != null && ProjectAccessRules.UserCanViewProject(role, uid, p))
                return;
        }

        if (article.ProjectTaskId.HasValue)
        {
            var t = await context.Tasks
                .Include(x => x.Project)
                .ThenInclude(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == article.ProjectTaskId.Value, cancellationToken);
            if (t != null && ProjectAccessRules.UserCanViewProject(role, uid, t.Project))
                return;
        }

        throw new ForbiddenException("Нет доступа к этой статье");
    }
}
