using Application.DTO.Projects;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById;

public class GetProjectByIdHandler(IKomSyncContext context, IMapper mapper) 
    : IRequestHandler<GetProjectByIdQuery, ProjectDetailedDto?>
{
    public async Task<ProjectDetailedDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Projects
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .ProjectTo<ProjectDetailedDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}