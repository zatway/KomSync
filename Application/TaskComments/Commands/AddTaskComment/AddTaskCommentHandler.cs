using Application.DTO.Task;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.TaskComments.Commands.AddTaskComment;

public class AddTaskCommentHandler(
    IKomSyncContext context,
    IMapper mapper,
    ICurrentUserService currentUserService
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

        // Добавляем всё в контекст
        context.TaskComments.Add(comment);
        
        await context.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}