using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>Колонка Kanban / статус задачи, настраиваемая для каждого проекта.</summary>
public class ProjectTaskStatusColumn
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    /// <summary>Порядок колонок слева направо.</summary>
    public int SortOrder { get; set; }

    [MaxLength(8)]
    public string? ColorHex { get; set; }

    /// <summary>Соответствие старому enum для миграции и аналитики: 0 Todo … 4 Blocked.</summary>
    public byte SemanticKind { get; set; }

    public bool IsDoneColumn { get; set; }
    public bool IsBlockedColumn { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
