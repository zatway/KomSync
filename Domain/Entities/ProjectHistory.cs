using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;

namespace Domain.Entities;

public class ProjectHistory : BaseEntity
{
    /// <summary>
    /// Проект
    /// </summary>
    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))] public Project Project { get; set; } = null!;

    /// <summary>
    /// Какое поле изменилось.
    /// </summary>
    [Required]
    public string Field { get; set; } = null!;

    /// <summary>
    /// Старое значение.
    /// </summary>
    [Required]
    public string OldValue { get; set; } = null!;

    /// <summary>
    /// Новое значение.
    /// </summary>
    [Required, MaxLength(30)]
    public string NewValue { get; set; }

    /// <summary>
    /// Инициатор изменения
    /// </summary>
    public Guid ChangedById { get; set; }

    [ForeignKey(nameof(ChangedById))] public User ChangedBy { get; set; } = null!;
}