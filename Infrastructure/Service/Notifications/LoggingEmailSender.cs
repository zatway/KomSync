using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service.Notifications;

public class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Email → {To} | {Subject} | {Body}", toEmail, subject, htmlBody);
        return Task.CompletedTask;
    }
}
