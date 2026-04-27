using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectHandler(IFmkSyncContext context, IMapper mapper)
        : IRequestHandler<UpdateProjectRequest, bool>
    {
        public async Task<bool> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                throw new NotFoundException("Проект не найден");

            mapper.Map(request, project);
            project.UpdateTimestamp();

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}