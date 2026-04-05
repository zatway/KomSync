namespace WebApi.Services;

public sealed class DeadlineReminderOptions
{
    public const string SectionName = "DeadlineReminders";

    public bool Enabled { get; set; } = true;

    public int IntervalHours { get; set; } = 1;

    public int[] OffsetsDays { get; set; } = [7, 3, 1, 0];
}
