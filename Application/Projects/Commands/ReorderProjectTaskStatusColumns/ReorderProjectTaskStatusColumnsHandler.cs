using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.ReorderProjectTaskStatusColumns;

public record ReorderProjectTaskStatusColumnsCommand(Guid ProjectId, IReadOnlyList<Guid> OrderedColumnIds)
    : IRequest<Unit>;

public class ReorderProjectTaskStatusColumnsHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<ReorderProjectTaskStatusColumnsCommand, Unit>
{
    public async Task<Unit> Handle(ReorderProjectTaskStatusColumnsCommand request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var project = await context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException("Проект не найден");

        if (!ProjectAccessRules.UserCanViewProject(role, uid, project, currentUser.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");
        if (!ProjectAccessRules.UserCanManageProjectsAndColumns(role))
            throw new ForbiddenException("Менять колонки могут только администратор или менеджер");

        var cols = await context.ProjectTaskStatusColumns
            .Where(c => c.ProjectId == request.ProjectId)
            .ToListAsync(cancellationToken);

        if (cols.Count == 0)
            return Unit.Value;

        if (request.OrderedColumnIds.Count != cols.Count)
            throw new BadRequestException("Список колонок не совпадает с проектом.");

        var idSet = cols.Select(c => c.Id).ToHashSet();
        if (request.OrderedColumnIds.Distinct().Count() != cols.Count ||
            request.OrderedColumnIds.Any(id => !idSet.Contains(id)))
            throw new BadRequestException("Некорректный порядок колонок.");

        for (var i = 0; i < request.OrderedColumnIds.Count; i++)
        {
            var col = cols.First(c => c.Id == request.OrderedColumnIds[i]);
            col.SortOrder = i;
        }

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
