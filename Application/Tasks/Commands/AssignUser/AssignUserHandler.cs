using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.AssignUser;

public class AssignUserHandler(
    IKomSyncContext context,
    ICurrentUserService currentUserService
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
        
        return true;
    }
}