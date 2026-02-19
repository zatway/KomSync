using Application.DTO.Projects;
using Application.DTO.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdHandler(IKomSyncContext context, IMapper mapper) 
    : IRequestHandler<GetTaskByIdQuery, TaskDetailedDto?>
{
    public async Task<TaskDetailedDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Tasks
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .ProjectTo<TaskDetailedDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}