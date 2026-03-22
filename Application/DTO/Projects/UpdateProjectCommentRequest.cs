using MediatR;

namespace Application.DTO.Projects;

public record UpdateProjectCommentRequest(
    Guid Id,
    string Content
) : IRequest<bool>;
