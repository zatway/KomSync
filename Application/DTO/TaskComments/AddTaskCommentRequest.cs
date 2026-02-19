using MediatR;

namespace Application.DTO.Task;

public record AddTaskCommentRequest(Guid TaskId, string Content) : IRequest<Guid>;