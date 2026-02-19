using MediatR;

namespace Application.DTO.TaskComments;

public record UpdateTaskCommentRequest(Guid Id, string Content) : IRequest<bool>;