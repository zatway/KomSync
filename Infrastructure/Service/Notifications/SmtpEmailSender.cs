using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Service.Notifications;

public record SmtpEmailSettings
{
    public bool Enabled { get; init; }
    public string Host { get; init; } = "";
    public int Port { get; init; } = 587;
    public bool UseSsl { get; init; } = true;
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
    public string FromEmail { get; init; } = "";
    public string FromName { get; init; } = "KomSync";
}

public class SmtpEmailSender(IOptions<SmtpEmailSettings> options) : IEmailSender
{
    private readonly SmtpEmailSettings _settings = options.Value;

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
            return;

        using var message = new MailMessage();
        message.From = new MailAddress(_settings.FromEmail, _settings.FromName);
        message.To.Add(new MailAddress(toEmail));
        message.Subject = subject;
        message.Body = htmlBody;
        message.IsBodyHtml = true;

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.UseSsl,
            Credentials = string.IsNullOrWhiteSpace(_settings.Username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(_settings.Username, _settings.Password),
        };

        // SmtpClient doesn't support CancellationToken directly
        await client.SendMailAsync(message);
    }
}

