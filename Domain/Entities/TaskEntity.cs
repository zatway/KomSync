using Domain.Entities.Common;
using Domain.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace Domain.Entities;

/// <summary>
/// Задача в системе. Может иметь статус, приоритет, исполнителя, крайний срок.
/// Поддерживает иерархию (подзадачи) и теги.
/// </summary>
public class TaskEntity : BaseEntity
{
    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Описание задачи (необязательно).
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Статус задачи, определяющий её текущий этап выполнения.
    /// По умолчанию — <see cref="System.Threading.Tasks.TaskStatus.Created" />.
    /// </summary>
    public TaskStatus Status { get; private set; } = TaskStatus.Created;

    /// <summary>
    /// Приоритет задачи, определяющий её важность.
    /// По умолчанию — <see cref="Enums.TaskPriority.Medium" />.
    /// </summary>
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;

    /// <summary>
    /// Идентификатор создателя задачи.
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// Идентификатор исполнителя задачи (необязательно).
    /// </summary>
    public Guid? AssigneeId { get; init; }

    /// <summary>
    /// Крайний срок выполнения задачи (необязательно).
    /// </summary>
    public DateTimeOffset? Deadline { get; init; }

    /// <summary>
    /// Идентификатор родительской задачи (для подзадач). Может быть null.
    /// </summary>
    public Guid? ParentTaskId { get; init; }

    /// <summary>
    /// Идентификатор проекта (заготовка на будущее). Может быть null.
    /// </summary>
    public Guid? ProjectId { get; init; }

    // Навигационные свойства

    /// <summary>
    /// Создатель задачи.
    /// </summary>
    public User Creator { get; private set; } = null!;

    /// <summary>
    /// Исполнитель задачи.
    /// </summary>
    public User? Assignee { get; init; }

    /// <summary>
    /// Родительская задача (если это подзадача).
    /// </summary>
    public TaskEntity? ParentTask { get; init; }

    /// <summary>
    /// Подзадачи.
    /// </summary>
    public ICollection<TaskEntity> Subtasks { get; private set; } = new List<TaskEntity>();

    /// <summary>
    /// Комментарии к задаче.
    /// </summary>
    public ICollection<TaskComment> Comments { get; private set; } = new List<TaskComment>();

    /// <summary>
    /// Теги, привязанные к задаче (связь многие-ко-многим).
    /// </summary>
    public ICollection<TaskTag> TaskTags { get; private set; } = new List<TaskTag>();

    /// <summary>
    /// Файлы, прикреплённые к задаче.
    /// </summary>
    public ICollection<TaskFile> TaskFiles { get; private set; } = new List<TaskFile>();
}