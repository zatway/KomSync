using MediatR;

namespace Application.DTO.Tasks;

public record DeleteTaskRequest(Guid Id) : IRequest<bool>;