using Application.DTO.Projects;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectsList;

public class GetProjectsListHandler(IKomSyncContext context, IMapper mapper) 
    : IRequestHandler<GetProjectsListQuery, List<ProjectBriefDto>>
{
    public async Task<List<ProjectBriefDto>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        return await context.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .ProjectTo<ProjectBriefDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}