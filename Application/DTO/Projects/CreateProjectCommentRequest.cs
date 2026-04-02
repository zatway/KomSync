using MediatR;

namespace Application.DTO.Projects;

public record CreateProjectCommentRequest(
    Guid ProjectId,
    string Content,
    Guid? ParentId,
    IReadOnlyList<Guid>? MentionsUserIds = null
) : IRequest<ProjectCommentDto>;