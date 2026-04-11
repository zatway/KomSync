using Domain.Entities;

namespace Application.Common;

public static class ProjectProgressCalculator
{
    /// <summary>Доля задач в колонках «готово», в процентах 0–100.</summary>
    public static decimal Compute(IReadOnlyCollection<ProjectTask> tasks)
    {
        if (tasks.Count == 0) return 0;
        var done = tasks.Count(t => t.StatusColumn is { IsDoneColumn: true });
        return Math.Round((decimal)done / tasks.Count * 100, 2);
    }
}
