using Application.DTO.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUserService,
    IRealtimeNotificationPublisher notifications
) : IRequestHandler<ChangeTaskStatusCommand, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await context.Tasks
            .FirstOrDefaultAsync(p => p.Id == request.TaskId, cancellationToken);

        if (task == null)
            return false;

        if (task.ProjectId != request.ProjectId)
            return false;

        var columnOk = await context.ProjectTaskStatusColumns
            .AnyAsync(c => c.Id == request.NewStatusColumnId && c.ProjectId == request.ProjectId, cancellationToken);
        if (!columnOk)
            return false;

        var columnChanged = task.ProjectTaskStatusColumnId != request.NewStatusColumnId;
        task.ProjectTaskStatusColumnId = request.NewStatusColumnId;

        if (request.NewSortOrder.HasValue)
            task.SortOrder = request.NewSortOrder.Value;
        else if (columnChanged)
        {
            var maxOrder = await context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == task.ProjectId
                    && t.ProjectTaskStatusColumnId == request.NewStatusColumnId
                    && t.Id != task.Id)
                .Select(t => (int?)t.SortOrder)
                .MaxAsync(cancellationToken) ?? -1;
            task.SortOrder = maxOrder + 1;
        }

        task.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        var actorId = currentUserService.UserId;
        var recipients = new HashSet<Guid> { task.CreatorId };
        if (task.AssigneeId.HasValue) recipients.Add(task.AssigneeId.Value);
        if (task.ResponsibleId.HasValue) recipients.Add(task.ResponsibleId.Value);
        if (actorId.HasValue) recipients.Remove(actorId.Value);

        foreach (var recipientId in recipients)
        {
            await notifications.PublishToUserAsync(
                recipientId,
                "task.status.changed",
                new { taskId = task.Id, newStatusColumnId = request.NewStatusColumnId, byUserId = actorId },
                cancellationToken);
        }

        return true;
    }
}
