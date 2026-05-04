using Application.Common.Exceptions;
using Application.DTO.TaskComments;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TaskComments.Commands.DeleteTaskComment;

public class DeleteTaskCommentHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteTaskCommentRequest, bool>
{
    public async Task<bool> Handle(DeleteTaskCommentRequest request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var root = await context.TaskComments
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (root == null)
            return false;

        var canDelete = root.UserId == uid
                        || role is UserRole.Admin or UserRole.Manager;
        if (!canDelete)
            throw new ForbiddenException("Нельзя удалить этот комментарий");

        await DeleteSubtreeAsync(request.Id, context, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static async Task DeleteSubtreeAsync(Guid id, IFmkSyncContext context, CancellationToken cancellationToken)
    {
        var childIds = await context.TaskComments
            .Where(c => c.ParentCommentId == id)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        foreach (var childId in childIds)
            await DeleteSubtreeAsync(childId, context, cancellationToken);

        var self = await context.TaskComments
            .Include(c => c.Attachments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (self == null)
            return;

        context.TaskCommentAttachments.RemoveRange(self.Attachments);
        context.TaskComments.Remove(self);
    }
}
