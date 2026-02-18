using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Группировка задач в рамках конкретного проекта
/// </summary>
public class Project
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Название проекта
    /// </summary>
    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Детальное описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;    
    
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Владелец проекта
    /// </summary>
    public Guid OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))] 
    public User Owner { get; set; } = null!;

    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}