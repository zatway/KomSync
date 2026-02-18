using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectHandler(
    IKomSyncContext context, 
    IMapper mapper) : IRequestHandler<CreateProjectRequest, Guid>
{
    public async Task<Guid> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = mapper.Map<Project>(request);

        // Добавляем в контекст
        context.Projects.Add(project);
        
        // Сохраняем (интерфейс IKomSyncContext должен иметь SaveChangesAsync)
        await context.SaveChangesAsync(cancellationToken);

        // 4. Возвращаем ID созданного проекта
        return project.Id;
    }
}