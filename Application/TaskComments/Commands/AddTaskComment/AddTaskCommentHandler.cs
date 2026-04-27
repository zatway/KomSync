using Application.DTO.TaskComments;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TaskComments.Commands.AddTaskComment;

public class AddTaskCommentHandler(
    IFmkSyncContext context,
    IMapper mapper,
    ICurrentUserService currentUserService,
    IRealtimeNotificationPublisher notifications
) : IRequestHandler<AddTaskCommentRequest, Guid>
{
    public async Task<Guid> Handle(AddTaskCommentRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");

        var comment = mapper.Map<TaskComment>(request);

        // Назначаем создателя
        comment.UserId = userId;
        comment.Id = Guid.NewGuid();
        comment.CreatedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;
        if (request.MentionsUserIds is { Count: > 0 })
            comment.MentionsUserIdsJson = System.Text.Json.JsonSerializer.Serialize(request.MentionsUserIds.Distinct());

        // Добавляем всё в контекст
        context.TaskComments.Add(comment);
        
        await context.SaveChangesAsync(cancellationToken);

        var task = await context.Tasks
            .Include(t => t.Watchers)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task != null)
        {
            var recipients = new HashSet<Guid>();
            recipients.Add(task.CreatorId);
            if (task.AssigneeId.HasValue) recipients.Add(task.AssigneeId.Value);
            if (task.ResponsibleId.HasValue) recipients.Add(task.ResponsibleId.Value);
            foreach (var w in task.Watchers) recipients.Add(w.UserId);
            if (request.ReplyToUserId.HasValue) recipients.Add(request.ReplyToUserId.Value);
            if (request.MentionsUserIds != null)
                foreach (var uid in request.MentionsUserIds) recipients.Add(uid);

            recipients.Remove(userId);

            foreach (var recipientId in recipients)
            {
                await notifications.PublishToUserAsync(
                    recipientId,
                    "task.comment.added",
                    new
                    {
                        taskId = task.Id,
                        commentId = comment.Id,
                        byUserId = userId,
                        isReply = request.ReplyToUserId.HasValue && request.ReplyToUserId.Value == recipientId,
                        isMention = request.MentionsUserIds?.Contains(recipientId) == true
                    },
                    cancellationToken);
            }
        }

        return comment.Id;
    }
}