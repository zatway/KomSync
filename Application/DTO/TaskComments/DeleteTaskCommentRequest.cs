using MediatR;

namespace Application.DTO.TaskComments;

public record DeleteTaskCommentRequest(Guid Id) : IRequest<bool>;