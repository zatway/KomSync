using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Лог изменений по задаче для аудита.
/// </summary>
public class TaskHistory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    public Guid ChangedById { get; set; }
    [ForeignKey(nameof(ChangedById))]
    public User ChangedBy { get; set; } = null!;

    // Название свойства (Title, Status, AssigneeId и т.д.)
    [Required, MaxLength(100)]
    public string PropertyName { get; set; } = null!;

    // Значение ДО изменения
    public string? OldValue { get; set; }

    // Значение ПОСЛЕ изменения
    public string? NewValue { get; set; }

    // Тип действия (enum, который мы создали ранее)
    public TaskHistoryAction Action { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}