using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.DeleteUserAdmin;

public class DeleteUserAdminHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteUserAdminCommand, bool>
{
    public async Task<bool> Handle(DeleteUserAdminCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null) return false;
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}