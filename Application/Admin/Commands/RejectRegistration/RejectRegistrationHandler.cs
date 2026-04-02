using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.RejectRegistration;

public record RejectRegistrationCommand(Guid ApplicationId) : IRequest<bool>;

public class RejectRegistrationHandler(IKomSyncContext context, ICurrentUserService currentUser)
    : IRequestHandler<RejectRegistrationCommand, bool>
{
    public async Task<bool> Handle(RejectRegistrationCommand request, CancellationToken cancellationToken)
    {
        _ = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var app = await context.ApplicationForRegistrations
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (app == null || app.User == null)
            return false;

        if (app.Status != RegistrationApplicationStatus.Pending)
            return false;

        app.Status = RegistrationApplicationStatus.Rejected;
        app.ProcessedAt = DateTimeOffset.UtcNow;
        app.ProcessedByUserId = currentUser.UserId;

        context.Users.Remove(app.User);
        context.ApplicationForRegistrations.Remove(app);

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
