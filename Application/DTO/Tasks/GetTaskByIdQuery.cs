using Application.DTO.Tasks;
using MediatR;

namespace Application.DTO.Tasks;

public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDetailedDto?>;