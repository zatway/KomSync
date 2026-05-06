using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Projects.Commands.AddProjectComment
{
    public class AddProjectCommentHandler(
        IKomSyncContext context,
        ICurrentUserService currentUser,
        IRealtimeNotificationPublisher notifications)
        : IRequestHandler<CreateProjectCommentRequest, ProjectCommentDto>
    {
        public async Task<ProjectCommentDto> Handle(CreateProjectCommentRequest request, CancellationToken cancellationToken)
        {
            var actorId = currentUser.UserId ?? throw new UnauthorizedAccessException("User not authorized");
            if (!TaskAccessRules.UserCanAddComments(currentUser.Role))
                throw new ForbiddenException("Читатель не может комментировать проект");

            var project = await context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken)
                ?? throw new NotFoundException("Проект не найден");
            if (!ProjectAccessRules.UserCanViewProject(currentUser.Role, actorId, project, currentUser.DepartmentId))
                throw new ForbiddenException("Нет доступа к проекту");

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == actorId, cancellationToken);

            if (user == null)
                throw new NotFoundException("Пользователь не найден");

            var comment = new ProjectComment
            {
                ProjectId = request.ProjectId,
                Content = request.Content,
                AuthorId = user.Id,
                ParentId = request.ParentId,
                MentionsUserIdsJson = request.MentionsUserIds is { Count: > 0 }
                    ? JsonSerializer.Serialize(request.MentionsUserIds.Distinct())
                    : null
            };

            await context.ProjectComments.AddAsync(comment, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // notify parent author + mentioned users (no broadcast)
            var recipients = new HashSet<Guid>();
            if (request.ParentId.HasValue)
            {
                var parentAuthorId = await context.ProjectComments
                    .AsNoTracking()
                    .Where(c => c.Id == request.ParentId.Value)
                    .Select(c => c.AuthorId)
                    .FirstOrDefaultAsync(cancellationToken);
                if (parentAuthorId != Guid.Empty) recipients.Add(parentAuthorId);
            }
            if (request.MentionsUserIds != null)
                foreach (var uid in request.MentionsUserIds) recipients.Add(uid);

            recipients.Remove(user.Id);

            foreach (var recipientId in recipients)
            {
                await notifications.PublishToUserAsync(
                    recipientId,
                    "project.comment.added",
                    new
                    {
                        projectId = comment.ProjectId,
                        commentId = comment.Id,
                        byUserId = user.Id,
                        isReply = request.ParentId.HasValue,
                        isMention = request.MentionsUserIds?.Contains(recipientId) == true
                    },
                    cancellationToken);
            }

            return new ProjectCommentDto(
                comment.Id,
                comment.ProjectId,
                comment.Content,
                new AuthorDto(user.Id, user.FullName, user.Email, user.Avatar != null),
                comment.CreatedAt,
                comment.UpdatedAt,
                comment.ParentId
            );
        }
    }
}