using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusHandler(
    IKomSyncContext context,
    ICurrentUserService currentUserService
) : IRequestHandler<ChangeTaskStatusCommand, bool>
{
    public async Task<bool> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        // 1. Ищем задачу
        var task = await context.Tasks
            .FirstOrDefaultAsync(p => p.Id == request.TaskId, cancellationToken);

        if (task == null) 
            return false; 
        
        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");
        
        task.Status = request.NewStatus;
        task.UpdatedAt = DateTime.UtcNow;
        
        // 3. Сохраняем изменения
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}