using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.CreateProjectTaskStatusColumn;

public record CreateProjectTaskStatusColumnCommand(
    Guid ProjectId,
    string Name,
    string? ColorHex,
    byte? SemanticKind,
    bool? IsDoneColumn,
    bool? IsBlockedColumn
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

        var existing = await context.ProjectTaskStatusColumns
            .Where(c => c.ProjectId == request.ProjectId)
            .ToListAsync(cancellationToken);

        var wantsDone = request.IsDoneColumn == true || request.SemanticKind == 3;
        var wantsBlocked = !wantsDone && (request.IsBlockedColumn == true || request.SemanticKind == 4);

        byte semantic;
        bool isDone;
        bool isBlocked;

        if (wantsDone)
        {
            semantic = 3;
            isDone = true;
            isBlocked = false;
        }
        else if (wantsBlocked)
        {
            semantic = 4;
            isDone = false;
            isBlocked = true;
        }
        else
        {
            semantic = request.SemanticKind ?? 255;
            if (semantic is not (0 or 1 or 2 or 255))
                semantic = 255;
            isDone = false;
            isBlocked = false;
        }

        if (isDone && existing.Any(TaskStatusColumnRules.IsDoneLike))
            throw new BadRequestException("В проекте уже есть завершающая колонка (закрывает задачи).");

        if (isBlocked && existing.Any(TaskStatusColumnRules.IsBlockedLike))
            throw new BadRequestException("В проекте уже есть колонка «заблокировано».");

        if (semantic == 0 && existing.Any(TaskStatusColumnRules.IsInitialLike))
            throw new BadRequestException("В проекте уже есть начальная колонка (backlog / «открыто»).");

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
            SemanticKind = semantic,
            IsDoneColumn = isDone,
            IsBlockedColumn = isBlocked,
        };

        context.ProjectTaskStatusColumns.Add(col);
        await context.SaveChangesAsync(cancellationToken);
        return col.Id;
    }
}
