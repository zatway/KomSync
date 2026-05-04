using Application.Common;
using Application.Common.Exceptions;
using Application.DTO.Projects;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectHistory;

public class GetProjectHistoryHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUser,
    IMapper mapper)
    : IRequestHandler<GetProjectHistoryQuery, List<ProjectHistoryEntryDto>>
{
    public async Task<List<ProjectHistoryEntryDto>> Handle(GetProjectHistoryQuery request, CancellationToken cancellationToken)
    {
        var uid = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var project = await context.Projects
            .Include(p => p.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken)
            ?? throw new NotFoundException("Проект не найден");
        if (!ProjectAccessRules.UserCanViewProject(currentUser.Role, uid, project, currentUser.DepartmentId))
            throw new ForbiddenException("Нет доступа к проекту");

        var history = await context.ProjectHistories
            .AsNoTracking()
            .Include(h => h.ChangedBy)
            .Where(h => h.ProjectId == request.ProjectId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ProjectHistoryEntryDto>>(history);
    }
}
