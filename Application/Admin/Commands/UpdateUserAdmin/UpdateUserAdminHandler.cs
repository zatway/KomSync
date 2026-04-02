using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.UpdateUserAdmin;

public class UpdateUserAdminHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<UpdateUserAdminCommand, bool>
{
    public async Task<bool> Handle(UpdateUserAdminCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null) return false;

        if (request.FullName != null) user.FullName = request.FullName.Trim();
        if (request.Email != null)
        {
            var email = request.Email.Trim();
            user.Email = email;
            user.NormalizedEmail = email.ToUpperInvariant();
        }
        if (request.IsApproved.HasValue) user.IsApproved = request.IsApproved.Value;
        if (request.Role.HasValue) user.Role = request.Role.Value;

        if (request.DepartmentId.HasValue) user.DepartmentId = request.DepartmentId.Value;
        if (request.PositionId.HasValue) user.PositionId = request.PositionId.Value;

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

