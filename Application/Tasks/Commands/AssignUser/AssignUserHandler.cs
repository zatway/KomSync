using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.AssignUser;

public class AssignUserHandler(
    IKomSyncContext context,
    ICurrentUserService currentUserService,
    IRealtimeNotificationPublisher notifications
) : IRequestHandler<AssignUserRequest, bool>
{
    public async Task<bool> Handle(AssignUserRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем задачу
        var task = await context.Tasks
            .FirstOrDefaultAsync(p => p.Id == request.TaskId, cancellationToken);

        if (task == null) 
            return false;
        
        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");
        
        task.AssigneeId = request.AssigneeId;
        task.UpdatedAt = DateTime.UtcNow;
        
        // 3. Сохраняем изменения
        await context.SaveChangesAsync(cancellationToken);

        if (request.AssigneeId.HasValue && request.AssigneeId.Value != userId)
        {
            await notifications.PublishToUserAsync(
                request.AssigneeId.Value,
                "task.assigned",
                new { taskId = task.Id, byUserId = userId },
                cancellationToken);
        }
        
        return true;
    }
}