using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProjectComment;

public class DeleteProjectCommentHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteProjectCommentRequest, bool>
{
    public async Task<bool> Handle(DeleteProjectCommentRequest request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var root = await context.ProjectComments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (root == null) return false;

        var canDelete = root.AuthorId == uid
                        || role is UserRole.Admin or UserRole.Manager;
        if (!canDelete)
            throw new ForbiddenException("Нельзя удалить этот комментарий");

        var allInProject = await context.ProjectComments
            .AsNoTracking()
            .Where(c => c.ProjectId == root.ProjectId)
            .Select(c => new { c.Id, c.ParentId })
            .ToListAsync(cancellationToken);

        var subtree = new HashSet<Guid> { request.Id };
        bool added;
        do
        {
            added = false;
            foreach (var row in allInProject)
            {
                if (row.ParentId.HasValue
                    && subtree.Contains(row.ParentId.Value)
                    && subtree.Add(row.Id))
                    added = true;
            }
        } while (added);

        var toDelete = await context.ProjectComments
            .Where(c => subtree.Contains(c.Id))
            .Include(c => c.Attachments)
            .ToListAsync(cancellationToken);

        foreach (var c in toDelete)
            context.ProjectCommentAttachments.RemoveRange(c.Attachments);

        context.ProjectComments.RemoveRange(toDelete);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
