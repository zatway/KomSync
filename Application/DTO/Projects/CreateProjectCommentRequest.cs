using MediatR;

namespace Application.DTO.Projects;

public record CreateProjectCommentRequest(
    Guid ProjectId,
    string Content,
    Guid? ParentId
) : IRequest<ProjectCommentDto>;