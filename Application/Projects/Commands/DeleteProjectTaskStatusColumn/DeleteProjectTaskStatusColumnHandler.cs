using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProjectTaskStatusColumn;

public record DeleteProjectTaskStatusColumnCommand(
    Guid ProjectId,
    Guid ColumnId,
    Guid? MoveTasksToColumnId
) : IRequest<Unit>;

public class DeleteProjectTaskStatusColumnHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteProjectTaskStatusColumnCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProjectTaskStatusColumnCommand request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var project = await context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException("Проект не найден");

        if (!ProjectAccessRules.UserCanViewProject(role, uid, project))
            throw new ForbiddenException("Нет доступа к проекту");

        var cols = await context.ProjectTaskStatusColumns
            .Where(c => c.ProjectId == request.ProjectId)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

        if (cols.Count <= 3)
            throw new BadRequestException("Нельзя удалить колонку: в проекте должно остаться минимум 3 этапа.");

        var column = cols.FirstOrDefault(c => c.Id == request.ColumnId)
                     ?? throw new NotFoundException("Колонка не найдена");

        Guid targetColumnId;
        if (request.MoveTasksToColumnId.HasValue)
        {
            var t = cols.FirstOrDefault(c => c.Id == request.MoveTasksToColumnId.Value);
            if (t == null || t.Id == column.Id)
                throw new BadRequestException("Укажите другую колонку для переноса задач.");
            targetColumnId = t.Id;
        }
        else
        {
            var idx = cols.FindIndex(c => c.Id == column.Id);
            if (idx > 0)
                targetColumnId = cols[idx - 1].Id;
            else
                targetColumnId = cols[idx + 1].Id;
        }

        var tasks = await context.Tasks
            .Where(t => t.ProjectTaskStatusColumnId == column.Id)
            .ToListAsync(cancellationToken);

        var maxOrder = await context.Tasks
            .Where(t => t.ProjectTaskStatusColumnId == targetColumnId)
            .Select(t => (int?)t.SortOrder)
            .MaxAsync(cancellationToken) ?? -1;

        foreach (var task in tasks)
        {
            task.ProjectTaskStatusColumnId = targetColumnId;
            maxOrder++;
            task.SortOrder = maxOrder;
        }

        context.ProjectTaskStatusColumns.Remove(column);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
