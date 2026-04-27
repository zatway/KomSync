using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(Guid UserId, UserRole Role) : IRequest<bool>;

public class UpdateUserRoleHandler(IFmkSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<UpdateUserRoleCommand, bool>
{
    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
            return false;

        user.Role = request.Role;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
