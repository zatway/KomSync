using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskHandler(
    IKomSyncContext context,
    IMapper mapper,
    ICurrentUserService currentUserService
) : IRequestHandler<CreateTaskRequest, Guid>
{
    public async Task<Guid> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
                     ?? throw new UnauthorizedAccessException("User must be logged in to create tasks.");

        var projectExists = await context.Projects.AnyAsync(p => p.Id == request.ProjectId, cancellationToken);
        if (!projectExists)
            throw new InvalidOperationException("Project not found.");

        var columnOk = await context.ProjectTaskStatusColumns
            .AnyAsync(c => c.Id == request.ProjectTaskStatusColumnId && c.ProjectId == request.ProjectId, cancellationToken);
        if (!columnOk)
            throw new InvalidOperationException("Invalid task status column for this project.");

        var task = mapper.Map<ProjectTask>(request);
        task.Id = Guid.NewGuid();
        task.CreatorId = userId;

        var maxNum = await context.Tasks
            .Where(t => t.ProjectId == request.ProjectId)
            .Select(t => (int?)t.TaskNumber)
            .MaxAsync(cancellationToken) ?? 0;

        task.TaskNumber = maxNum + 1;
        task.SortOrder = maxNum + 1;

        context.Tasks.Add(task);

        foreach (var watcherId in request.WatcherUserIds ?? Array.Empty<Guid>())
        {
            context.ProjectTaskWatchers.Add(new ProjectTaskWatcher
            {
                TaskId = task.Id,
                UserId = watcherId
            });
        }

        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
