using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApi.Services;

public sealed class DeadlineReminderHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<DeadlineReminderOptions> options,
    ILogger<DeadlineReminderHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opt = options.Value;
        if (!opt.Enabled)
            return;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<IKomSyncContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var notifications = scope.ServiceProvider.GetRequiredService<IRealtimeNotificationPublisher>();
                await RunOnceAsync(context, email, notifications, opt, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка напоминаний о дедлайнах");
            }

            var delay = TimeSpan.FromHours(Math.Max(1, opt.IntervalHours));
            await Task.Delay(delay, stoppingToken);
        }
    }

    private static async Task RunOnceAsync(
        IKomSyncContext context,
        IEmailSender email,
        IRealtimeNotificationPublisher notifications,
        DeadlineReminderOptions opt,
        CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var offsets = opt.OffsetsDays?.Length > 0 ? opt.OffsetsDays : [7, 3, 1, 0];

        var tasks = await context.Tasks
            .Include(t => t.StatusColumn)
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Where(t => t.AssigneeId != null && t.Deadline != null && !t.StatusColumn.IsDoneColumn)
            .ToListAsync(ct);

        foreach (var t in tasks)
        {
            if (t.Assignee == null || t.Deadline == null) continue;

            var deadline = DateOnly.FromDateTime(t.Deadline.Value);
            var days = deadline.DayNumber - today.DayNumber;

            string logKey;
            if (days < 0)
                logKey = $"{t.Id:N}|overdue|{today:yyyy-MM-dd}";
            else if (offsets.Contains(days))
                logKey = $"{t.Id:N}|d{days}";
            else
                continue;

            var exists = await context.DeadlineReminderLogs.AnyAsync(l => l.LogKey == logKey, ct);
            if (exists) continue;

            context.DeadlineReminderLogs.Add(new DeadlineReminderLog
            {
                Id = Guid.NewGuid(),
                LogKey = logKey,
                ProjectTaskId = t.Id,
                BucketDays = days,
                SentOnDateUtc = today,
                SentAtUtc = DateTimeOffset.UtcNow
            });

            var subject = days < 0
                ? $"Просрочена задача {t.Project.Key}-{t.TaskNumber}"
                : $"Напоминание: дедлайн задачи {t.Project.Key}-{t.TaskNumber} через {days} дн.";

            var body =
                $"<p><b>{System.Net.WebUtility.HtmlEncode(t.Title)}</b></p>" +
                $"<p>Проект: {System.Net.WebUtility.HtmlEncode(t.Project.Name)}</p>" +
                $"<p>Дедлайн: {deadline:yyyy-MM-dd}</p>";

            await email.SendAsync(t.Assignee.Email, subject, body, ct);
            await notifications.PublishToUserAsync(
                t.AssigneeId!.Value,
                "deadline",
                new
                {
                    taskId = t.Id,
                    projectId = t.ProjectId,
                    title = t.Title,
                    daysUntil = days
                },
                ct);

            await context.SaveChangesAsync(ct);
        }
    }
}
