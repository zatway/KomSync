using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProjectTaskStatusColumn;

public record UpdateProjectTaskStatusColumnCommand(
    Guid ProjectId,
    Guid ColumnId,
    string Name,
    string? ColorHex
) : IRequest<Unit>;

public class UpdateProjectTaskStatusColumnHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<UpdateProjectTaskStatusColumnCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProjectTaskStatusColumnCommand request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var role = currentUser.Role;

        var project = await context.Projects
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

        if (project == null)
            throw new NotFoundException("Проект не найден");

        if (!ProjectAccessRules.UserCanViewProject(role, uid, project, currentUser.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");
        if (!ProjectAccessRules.UserCanManageProjectsAndColumns(role))
            throw new ForbiddenException("Редактировать колонки могут только администратор или менеджер");

        var col = await context.ProjectTaskStatusColumns
            .FirstOrDefaultAsync(
                c => c.Id == request.ColumnId && c.ProjectId == request.ProjectId,
                cancellationToken);

        if (col == null)
            throw new NotFoundException("Колонка не найдена");

        var name = request.Name.Trim();
        if (string.IsNullOrEmpty(name) || name.Length > 120)
            throw new BadRequestException("Название колонки: 1–120 символов.");

        col.Name = name;
        col.ColorHex = string.IsNullOrWhiteSpace(request.ColorHex) ? null : request.ColorHex.Trim();

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
