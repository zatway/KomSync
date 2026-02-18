using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskHandler(
    IKomSyncContext context,
    IMapper mapper,
    ICurrentUserService currentUserService
) : IRequestHandler<UpdateTaskRequest, bool>
{
    public async Task<bool> Handle(UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем задачу
        var task = await context.Tasks
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (task == null)
            return false;

        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");

        // 2. Обновляем существующую сущность данными из запроса
        mapper.Map(request, task);

        task.UpdatedAt = DateTime.UtcNow;

        // 3. Сохраняем изменения
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}