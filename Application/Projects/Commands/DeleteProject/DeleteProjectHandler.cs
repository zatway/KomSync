using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandler(IKomSyncContext context) : IRequestHandler<DeleteProjectRequest, bool>
    {
        public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                throw new NotFoundException("Проект не найден");

            context.Projects.Remove(project);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}