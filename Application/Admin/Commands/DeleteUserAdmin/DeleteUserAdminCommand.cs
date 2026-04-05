using MediatR;

namespace Application.Admin.Commands.DeleteUserAdmin;

public record DeleteUserAdminCommand(
    Guid UserId
) : IRequest<bool>;