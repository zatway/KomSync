using Application.DTO.Projects;
using Application.DTO.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskHandler(IKomSyncContext context) 
    : IRequestHandler<DeleteTaskRequest, bool>
{
    public async Task<bool> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем задачу в базе
        var task = await context.Tasks
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (task == null)
            return false;

        // 2. Удаляем
        context.Tasks.Remove(task);
        
        // 3. Сохраняем
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}