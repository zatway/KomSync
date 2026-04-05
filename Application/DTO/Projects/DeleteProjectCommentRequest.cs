using MediatR;

namespace Application.DTO.Projects;

public record DeleteProjectCommentRequest(Guid Id) : IRequest<bool>;
