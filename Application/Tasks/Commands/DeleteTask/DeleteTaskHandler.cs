using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteTaskRequest, bool>
{
    public async Task<bool> Handle(DeleteTaskRequest request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var task = await context.Tasks
            .Include(t => t.Project)
            .ThenInclude(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (task == null)
            return false;

        if (!ProjectAccessRules.UserCanViewProject(currentUser.Role, uid, task.Project, currentUser.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");
        if (!TaskAccessRules.UserCanModifyTask(currentUser.Role, uid, task))
            throw new ForbiddenException("Недостаточно прав для удаления задачи");

        context.Tasks.Remove(task);
        
        // 3. Сохраняем
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }
}