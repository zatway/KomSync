using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>Факт отправки напоминания по дедлайну (LogKey уникален: d7|taskId, overdue|taskId|yyyy-MM-dd).</summary>
public class DeadlineReminderLog
{
    public Guid Id { get; set; }

    [Required, MaxLength(96)]
    public string LogKey { get; set; } = null!;

    public Guid ProjectTaskId { get; set; }

    [ForeignKey(nameof(ProjectTaskId))]
    public ProjectTask ProjectTask { get; set; } = null!;

    /// <summary>7, 3, 1, 0 (день дедлайна) или -1 (просрочка за конкретный календарный день).</summary>
    public int BucketDays { get; set; }

    public DateOnly SentOnDateUtc { get; set; }

    public DateTimeOffset SentAtUtc { get; set; }
}
