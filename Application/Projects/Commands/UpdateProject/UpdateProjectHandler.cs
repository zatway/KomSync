using Application.DTO.Projects;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectHandler(
    IKomSyncContext context, 
    IMapper mapper) : IRequestHandler<UpdateProjectRequest, bool>
{
    public async Task<bool> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем проект
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project == null) 
            return false; 

        // 2. Обновляем существующую сущность данными из запроса
        mapper.Map(request, project);

        project.UpdatedAt = DateTime.UtcNow;

        // 3. Сохраняем изменения
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}