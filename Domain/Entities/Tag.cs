using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Тег для задач. Используется для категоризации и фильтрации.
/// Может иметь цвет для отображения в интерфейсе.
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// Название тега. Должно быть уникальным.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// HEX-цвет тега (например, "#3B82F6") для отображения в UI.
    /// </summary>
    public string? Color { get; private set; }

    // Навигационные свойства

    /// <summary>
    /// Связи с задачами (связь многие-ко-многим).
    /// </summary>
    public ICollection<TaskTag> TaskTags { get; private set; } = new List<TaskTag>();
}