using Application.DTO.Projects;
using Application.DTO.TaskComments;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TaskComments.Commands.UpdateTaskComment;

public class UpdateTaskCommentHandler(
    IKomSyncContext context, 
    IMapper mapper) : IRequestHandler<UpdateTaskCommentRequest, bool>
{
    public async Task<bool> Handle(UpdateTaskCommentRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем комментацрий
        var taskComment = await context.TaskComments
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (taskComment == null) 
            return false; 

        // 2. Обновляем существующую сущность данными из запроса
        mapper.Map(request, taskComment);

        taskComment.UpdatedAt = DateTime.UtcNow;

        // 3. Сохраняем изменения
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}