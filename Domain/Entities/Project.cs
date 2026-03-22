using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

/// <summary>
/// Группировка задач в рамках конкретного проекта
/// </summary>
public class Project : BaseEntity
{
    /// <summary>
    /// Ключ проекта
    /// </summary>
    [Required, MaxLength(30)]
    public string Key { get; set; } = null!;  
    
    /// <summary>
    /// Название проекта
    /// </summary>
    [Required, MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Детальное описание
    /// </summary>
    [Required, MaxLength(200)]
    public string Description { get; set; }
    
    /// <summary>
    /// Дата и время начала проекта.
    /// </summary>
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;    
    
    /// <summary>
    /// Дата и время дедлайна проекта.
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }    
    
    /// <summary>
    /// Дата и время завершения проекта.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// Цвет проекта.
    /// </summary>
    [Required, MaxLength(6)]
    public string Color { get; set; } = "0000ff";
    
    /// <summary>
    /// Эмодзи проекта.
    /// </summary>
    public char? Icon { get; set; }

    /// <summary>
    /// Владелец проекта
    /// </summary>
    public Guid OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))] 
    public User Owner { get; set; } = null!;
    
    public ICollection<User> Members { get; set; } = new List<User>();
    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    
    public ICollection<ProjectComment> ProjectComments { get; set; } = new List<ProjectComment>();
    public ICollection<ProjectHistory> ProjectHistories { get; set; } = new List<ProjectHistory>();
    
    /// <summary>
    /// Прогресс выполнения проекта
    /// </summary>
    [Required, Range(0, 100)]
    public decimal Progress { get; set; }
    
    /// <summary>
    /// Отдел который выполняет проект
    /// </summary>
    public Guid DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))] 
    public Department Department { get; set; } = null!;
}