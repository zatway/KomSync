using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Комментарий к задаче. Используется для обсуждения задачи.
/// Поддерживает упоминания пользователей (@username).
/// </summary>
public class TaskComment : BaseEntity
{
    /// <summary>
    /// Идентификатор задачи, к которой оставлен комментарий.
    /// </summary>
    public Guid TaskId { get; init; }

    /// <summary>
    /// Идентификатор автора комментария.
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Текст комментария.
    /// </summary>
    public string Content { get; private set; } = null!;

    // Навигационные свойства

    /// <summary>
    /// Задача, к которой относится комментарий.
    /// </summary>
    public TaskEntity Task { get; private set; } = null!;

    /// <summary>
    /// Автор комментария.
    /// </summary>
    public User Author { get; private set; } = null!;
}