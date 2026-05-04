using Domain.Entities;

namespace Application.Common;

/// <summary>Правила классификации колонок статусов (совместимость IsDoneColumn / SemanticKind).</summary>
public static class TaskStatusColumnRules
{
    public static bool IsDoneLike(ProjectTaskStatusColumn c) => c.IsDoneColumn || c.SemanticKind == 3;

    public static bool IsBlockedLike(ProjectTaskStatusColumn c) => c.IsBlockedColumn || c.SemanticKind == 4;

    /// <summary>«Начальная» колонка: backlog / todo (SemanticKind 0), не завершение и не блокировка.</summary>
    public static bool IsInitialLike(ProjectTaskStatusColumn c) =>
        c.SemanticKind == 0 && !IsDoneLike(c) && !IsBlockedLike(c);
}
