using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectById;

public class GetProjectByIdHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetProjectByIdQuery, ProjectDetailedDto>
{
    public async Task<ProjectDetailedDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
            .Include(p => p.Tasks).ThenInclude(t => t.StatusColumn)
            .Include(p => p.Tags)
            .Include(p => p.Categories)
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (project == null)
            throw new NotFoundException("Проект не найден");

        var currentUserId = currentUser.UserId ?? Guid.Empty;

        var tasks = project.Tasks.ToList();

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
                tasks.Count,
                tasks.Count(t => !t.StatusColumn.IsDoneColumn),
                tasks.Count(t => t.StatusColumn.SemanticKind == 1),
                tasks.Count(t => t.StatusColumn.SemanticKind == 2),
                tasks.Count(t => t.StatusColumn.IsDoneColumn),
                tasks.Count(t => t.StatusColumn.IsBlockedColumn),
                0
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
