using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Лог изменений по задаче для аудита.
/// </summary>
public class TaskHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    public Guid ChangedById { get; set; }
    [ForeignKey(nameof(ChangedById))]
    public User ChangedBy { get; set; } = null!;

    /// <summary> Имя измененного свойства (напр. Status). </summary>
    [Required, MaxLength(100)]
    public string PropertyName { get; set; } = null!;

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
