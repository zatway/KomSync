namespace Domain.Enums;

/// <summary>
/// Перечисление статусов задачи в системе трекера.
/// Определяет текущий этап выполнения задачи.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Задача создана, но не начата.
    /// </summary>
    ToDo,

    /// <summary>
    /// Задача в процессе выполнения.
    /// </summary>
    InProgress,

    /// <summary>
    /// Задача выполнена, находится на проверке.
    /// </summary>
    Review,

    /// <summary>
    /// Задача завершена и принята.
    /// </summary>
    Done
}