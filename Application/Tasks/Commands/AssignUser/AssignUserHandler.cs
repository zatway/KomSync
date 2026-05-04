using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.AssignUser;

public class AssignUserHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUserService,
    IRealtimeNotificationPublisher notifications
) : IRequestHandler<AssignUserRequest, bool>
{
    public async Task<bool> Handle(AssignUserRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");

        var task = await context.Tasks
            .Include(t => t.Project)
            .ThenInclude(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.TaskId, cancellationToken);

        if (task == null)
            return false;

        if (!ProjectAccessRules.UserCanViewProject(
                currentUserService.Role, userId, task.Project, currentUserService.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");
        if (!TaskAccessRules.UserCanModifyTask(currentUserService.Role, userId, task))
            throw new ForbiddenException("Недостаточно прав для назначения исполнителя");

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