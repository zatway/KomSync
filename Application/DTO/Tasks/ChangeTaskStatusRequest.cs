using MediatR;

namespace Application.DTO.Tasks;

public record ChangeTaskStatusCommand(
    Guid TaskId,
    Guid ProjectId,
    Guid NewStatusColumnId,
    int? NewSortOrder
) : IRequest<bool>;
