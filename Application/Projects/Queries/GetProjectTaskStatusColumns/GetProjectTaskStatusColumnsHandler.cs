using Application.DTO.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectTaskStatusColumns;

public record GetProjectTaskStatusColumnsQuery(Guid ProjectId) : IRequest<IReadOnlyList<TaskStatusColumnDto>>;

public class GetProjectTaskStatusColumnsHandler(IKomSyncContext context)
    : IRequestHandler<GetProjectTaskStatusColumnsQuery, IReadOnlyList<TaskStatusColumnDto>>
{
    public async Task<IReadOnlyList<TaskStatusColumnDto>> Handle(
        GetProjectTaskStatusColumnsQuery request,
        CancellationToken cancellationToken)
    {
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
