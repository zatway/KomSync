using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandler(IKomSyncContext context, ICurrentUserService currentUser)
        : IRequestHandler<GetProjectByIdQuery, ProjectDetailedDto>
    {
        public async Task<ProjectDetailedDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.Tasks)
                .Include(p => p.Tags)
                .Include(p => p.Categories)
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                throw new Exception("Project not found");

            var currentUserId = currentUser.UserId ?? Guid.Empty;

            return new ProjectDetailedDto(
                project.Id,
                project.Key,
                project.Name,
                project.Description,
                project.StartDate,
                project.DueDate,
                project.CompletedAt,
                project.CreatedAt,
                project.UpdatedAt,
                project.Color,
                project.Icon?.ToString(),
                new OwnerDto(project.OwnerId, project.Owner.FullName, project.Owner.Email, project.Owner.Role),
                project.Members.Select(m => new MemberDto(m.Id, m.FullName, m.Email, m.Role)).ToList(),
                new TaskStatsDto(
                    project.Tasks.Count,
                    project.Tasks.Count(t =>  t.Status != ProjectTaskStatus.Done),
                    project.Tasks.Count(t => t.Status == ProjectTaskStatus.InProgress),
                    project.Tasks.Count(t => t.Status ==ProjectTaskStatus.Review),
                    project.Tasks.Count(t => t.Status == ProjectTaskStatus.Done),
                    //TODO ДОПИСАТТЬ ПОТОМ
                    project.Tasks.Count(t => t.Status == ProjectTaskStatus.Review),
                    project.Tasks.Count(t =>  t.Status == ProjectTaskStatus.Review)
                ),
                project.Progress,
                project.Tags.Select(t => t.Name).ToList(),
                project.Categories.FirstOrDefault()?.Name,
                project.Department?.Name,
                false,
                new PermissionsDto(
                    CanEdit: project.OwnerId == currentUserId,
                    CanDelete: project.OwnerId == currentUserId,
                    CanManageMembers: project.OwnerId == currentUserId,
                    CanViewHistory: true
                ),
                null
            );
        }
    }
}