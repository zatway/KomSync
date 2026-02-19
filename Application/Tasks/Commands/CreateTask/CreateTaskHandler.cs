using Application.DTO.Tasks;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;

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

        var task = mapper.Map<ProjectTask>(request);

        // Назначаем создателя
        task.CreatorId = userId;

        // Добавляем всё в контекст
        context.Tasks.Add(task);
        
        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}