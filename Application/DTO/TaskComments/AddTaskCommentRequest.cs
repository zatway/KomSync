using MediatR;

namespace Application.DTO.TaskComments;

public record AddTaskCommentRequest(
    Guid TaskId,
    string Content,
    IReadOnlyList<Guid>? MentionsUserIds = null,
    Guid? ReplyToUserId = null,
    Guid? ParentCommentId = null
) : IRequest<Guid>;