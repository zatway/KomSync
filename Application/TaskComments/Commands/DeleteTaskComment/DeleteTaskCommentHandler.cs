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

        var taskComment = await context.TaskComments
            .Include(c => c.Attachments)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (taskComment == null)
            return false;

        var canDelete = taskComment.UserId == uid
                        || role is UserRole.Admin or UserRole.Manager;
        if (!canDelete)
            throw new ForbiddenException("Нельзя удалить этот комментарий");

        context.TaskCommentAttachments.RemoveRange(taskComment.Attachments);
        context.TaskComments.Remove(taskComment);

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
