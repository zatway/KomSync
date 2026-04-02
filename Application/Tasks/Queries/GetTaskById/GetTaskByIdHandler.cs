using Application.DTO.TaskComment;
using Application.DTO.TaskHistory;
using Application.DTO.Tasks;
using Application.DTO.Attachments;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdHandler(IKomSyncContext context, IMapper mapper)
    : IRequestHandler<GetTaskByIdQuery, TaskDetailedDto?>
{
    public async Task<TaskDetailedDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await context.Tasks
            .AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.StatusColumn)
            .Include(t => t.Assignee)
            .Include(t => t.Responsible)
            .Include(t => t.Comments).ThenInclude(c => c.User)
            .Include(t => t.Comments).ThenInclude(c => c.Attachments)
            .Include(t => t.History)
            .Include(t => t.Watchers).ThenInclude(w => w.User)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (task == null)
            return null;

        var dto = mapper.Map<TaskDetailedDto>(task);
        dto.Key = $"{task.Project.Key}-{task.TaskNumber}";
        var commentDtos = mapper.Map<TaskCommentDto[]>(task.Comments.OrderBy(c => c.CreatedAt).ToArray());
        foreach (var c in commentDtos)
        {
            var entity = task.Comments.First(e => e.Id == c.Id);
            c.Attachments = mapper.Map<CommentAttachmentDto[]>(entity.Attachments.OrderBy(a => a.CreatedAt).ToArray());
        }
        dto.Comments = commentDtos;
        dto.History = mapper.Map<TaskHistoryDto[]>(task.History.OrderByDescending(h => h.ChangedAt).ToArray());
        dto.Watchers = task.Watchers
            .Select(w => new TaskAssigneeDto(w.User.Id, w.User.FullName, null))
            .ToList();

        return dto;
    }
}
