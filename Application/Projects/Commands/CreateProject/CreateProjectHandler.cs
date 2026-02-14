using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectHandler(
    IKomSyncContext context, 
    IMapper mapper) : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // 1. Маппим команду в сущность Domain
        var project = mapper.Map<Project>(request);

        // 2. Добавляем в контекст
        context.Projects.Add(project);
        
        // 3. Сохраняем (интерфейс IKomSyncContext должен иметь SaveChangesAsync)
        await context.SaveChangesAsync(cancellationToken);

        // 4. Возвращаем ID созданного проекта
        return project.Id;
    }
}