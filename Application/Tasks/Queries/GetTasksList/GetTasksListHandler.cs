using Application.DTO.Tasks;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries.GetTasksList;

public class GetTasksListHandler(IKomSyncContext context, IMapper mapper)
    : IRequestHandler<GetTasksListQuery, List<TaskShortDto>>
{
    public async Task<List<TaskShortDto>> Handle(GetTasksListQuery request, CancellationToken cancellationToken)
    {
        var tasks = await context.Tasks
            .AsNoTracking()
            .Where(x => x.ProjectId == request.ProjectId)
            .Include(t => t.Project)
            .Include(t => t.StatusColumn)
            .Include(t => t.Assignee)
            .Include(t => t.Responsible)
            .OrderBy(t => t.StatusColumn.SortOrder)
            .ThenBy(t => t.SortOrder)
            .ThenBy(t => t.TaskNumber)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<TaskShortDto>>(tasks);
    }
}
