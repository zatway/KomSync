using Application.DTO.Projects;
using Application.DTO.TaskComments;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TaskComments.Commands.DeleteTaskComment;

public class DeleteTaskCommentHandler(IKomSyncContext context) 
    : IRequestHandler<DeleteTaskCommentRequest, bool>
{
    public async Task<bool> Handle(DeleteTaskCommentRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем комментацрий в базе
        var taskComment = await context.TaskComments
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (taskComment == null)
            return false;

        // 2. Удаляем
        context.TaskComments.Remove(taskComment);
        
        // 3. Сохраняем
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}