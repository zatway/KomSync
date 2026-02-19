using Application.DTO.Tasks;
using MediatR;

namespace Application.DTO.Tasks;

public record GetTasksListQuery(Guid ProjectId) : IRequest<List<TaskShortDto>>;