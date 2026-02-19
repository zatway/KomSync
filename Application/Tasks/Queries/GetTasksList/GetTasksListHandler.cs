using Application.DTO.Projects;
using Application.DTO.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries.GetTasksList;

public class GetTasksListHandler(IKomSyncContext context, IMapper mapper) 
    : IRequestHandler<GetTasksListQuery, List<TaskShortDto>>
{
    public async Task<List<TaskShortDto>> Handle(GetTasksListQuery request, CancellationToken cancellationToken)
    {
        return await context.Tasks
            .AsNoTracking()
            .Where(x => x.ProjectId == request.ProjectId)
            .OrderByDescending(p => p.CreatedAt)
            .ProjectTo<TaskShortDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}