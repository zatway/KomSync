using Application.DTO.Projects;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Queries.GetProjectHistory
{
    public class GetProjectHistoryHandler(IFmkSyncContext context)
        : IRequestHandler<GetProjectHistoryQuery, List<ProjectHistoryEntryDto>>
    {
        public async Task<List<ProjectHistoryEntryDto>> Handle(GetProjectHistoryQuery request, CancellationToken cancellationToken)
        {
            var history = await context.ProjectHistories
                .Include(h => h.ChangedBy)
                .Where(h => h.ProjectId == request.ProjectId)
                .OrderByDescending(h => h.UpdatedAt)
                .ToListAsync(cancellationToken);

            return history.Select(h => new ProjectHistoryEntryDto(
                h.Id,
                h.Field,
                h.OldValue,
                h.NewValue,
                new ChangedByDto(h.ChangedById ?? Guid.Empty, h.ChangedBy.FullName),
                h.CreatedAt
            )).ToList();
        }
    }
}