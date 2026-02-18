using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProject;

public class DeleteProjectHandler(IKomSyncContext context) 
    : IRequestHandler<DeleteProjectRequest, bool>
{
    public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        // 1. Ищем проект в базе
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project == null)
            return false;

        // 2. Удаляем
        context.Projects.Remove(project);
        
        // 3. Сохраняем
        await context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}