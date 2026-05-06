using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectTaskStatusColumns;

public record GetProjectTaskStatusColumnsQuery(Guid ProjectId) : IRequest<IReadOnlyList<TaskStatusColumnDto>>;

public class GetProjectTaskStatusColumnsHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetProjectTaskStatusColumnsQuery, IReadOnlyList<TaskStatusColumnDto>>
{
    public async Task<IReadOnlyList<TaskStatusColumnDto>> Handle(
        GetProjectTaskStatusColumnsQuery request,
        CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var project = await context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
        if (project == null)
            throw new NotFoundException("Проект не найден");
        if (!ProjectAccessRules.UserCanViewProject(currentUser.Role, uid, project, currentUser.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");

        var cols = await context.ProjectTaskStatusColumns
            .AsNoTracking()
            .Where(c => c.ProjectId == request.ProjectId)
            .OrderBy(c => c.SortOrder)
            .Select(c => new TaskStatusColumnDto(
                c.Id,
                c.Name,
                c.SortOrder,
                c.ColorHex,
                c.SemanticKind,
                c.IsDoneColumn,
                c.IsBlockedColumn))
            .ToListAsync(cancellationToken);

        return cols;
    }
}
