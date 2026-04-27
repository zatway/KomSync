using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.ApproveRegistration;

public record ApproveRegistrationCommand(Guid ApplicationId) : IRequest<bool>;

public class ApproveRegistrationHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUser,
    IEmailSender emailSender,
    IRealtimeNotificationPublisher notifications
) : IRequestHandler<ApproveRegistrationCommand, bool>
{
    public async Task<bool> Handle(ApproveRegistrationCommand request, CancellationToken cancellationToken)
    {
        var app = await context.ApplicationForRegistrations
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (app == null || app.User == null)
            return false;

        if (app.Status != RegistrationApplicationStatus.Pending)
            return false;

        var adminId = currentUser.UserId ?? throw new UnauthorizedAccessException();

        app.User.IsApproved = true;
        app.User.Role = app.RequestedRole;
        app.Status = RegistrationApplicationStatus.Approved;
        app.ProcessedAt = DateTimeOffset.UtcNow;
        app.ProcessedByUserId = adminId;

        await context.SaveChangesAsync(cancellationToken);

        await emailSender.SendAsync(
            app.User.Email,
            "FmkSync — регистрация одобрена",
            $"<p>Здравствуйте, {app.User.FullName}!</p><p>Ваша учётная запись активирована. Можно войти в систему.</p>",
            cancellationToken);

        await notifications.PublishToUserAsync(
            app.User.Id,
            "registration.approved",
            new { message = "Регистрация одобрена", applicationId = app.Id },
            cancellationToken);

        return true;
    }
}
