using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandler(IKomSyncContext context, ICurrentUserService currentUser)
        : IRequestHandler<DeleteProjectRequest, bool>
    {
        public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
            var project = await context.Projects
                .Include(p => p.Members)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                throw new NotFoundException("Проект не найден");

            if (!ProjectAccessRules.UserCanViewProject(currentUser.Role, uid, project, currentUser.DepartmentId))
                throw new ForbiddenException("Нет доступа к проекту");
            if (!ProjectAccessRules.UserCanManageProjectsAndColumns(currentUser.Role))
                throw new ForbiddenException("Удалять проект могут только администратор или менеджер");

            context.Projects.Remove(project);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}