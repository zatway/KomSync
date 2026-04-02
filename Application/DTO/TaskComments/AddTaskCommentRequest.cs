using MediatR;

namespace Application.DTO.TaskComments;

public record AddTaskCommentRequest(
    Guid TaskId,
    string Content,
    IReadOnlyList<Guid>? MentionsUserIds = null,
    Guid? ReplyToUserId = null
) : IRequest<Guid>;