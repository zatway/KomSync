using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.CreateProjectTaskStatusColumn;

public record CreateProjectTaskStatusColumnCommand(
    Guid ProjectId,
    string Name,
    string? ColorHex
) : IRequest<Guid>;

public class CreateProjectTaskStatusColumnHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<CreateProjectTaskStatusColumnCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectTaskStatusColumnCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var exists = await context.Projects.AnyAsync(p => p.Id == request.ProjectId, cancellationToken);
        if (!exists)
            throw new NotFoundException("Проект не найден");

        var columnCount = await context.ProjectTaskStatusColumns
            .CountAsync(c => c.ProjectId == request.ProjectId, cancellationToken);
        if (columnCount >= 10)
            throw new BadRequestException("В проекте может быть не более 10 колонок статусов.");

        var maxOrder = await context.ProjectTaskStatusColumns
            .Where(c => c.ProjectId == request.ProjectId)
            .Select(c => (int?)c.SortOrder)
            .MaxAsync(cancellationToken) ?? -1;

        var col = new ProjectTaskStatusColumn
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Name = request.Name.Trim(),
            SortOrder = maxOrder + 1,
            ColorHex = request.ColorHex,
            SemanticKind = 255,
            IsDoneColumn = false,
            IsBlockedColumn = false
        };

        context.ProjectTaskStatusColumns.Add(col);
        await context.SaveChangesAsync(cancellationToken);
        return col.Id;
    }
}
