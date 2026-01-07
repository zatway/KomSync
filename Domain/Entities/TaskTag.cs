namespace Domain.Entities;

/// <summary>
/// Составная сущность, представляющая связь многие-ко-многим между задачей и тегом.
/// Используется для привязки тегов к задачам.
/// </summary>
public class TaskTag
{
    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public Guid TaskId { get; private set; }

    /// <summary>
    /// Идентификатор тега.
    /// </summary>
    public Guid TagId { get; private set; }

    // Навигационные свойства

    /// <summary>
    /// Ссылка на задачу.
    /// </summary>
    public TaskEntity Task { get; private set; } = null!;

    /// <summary>
    /// Ссылка на тег.
    /// </summary>
    public Tag Tag { get; private set; } = null!;
}