using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>Наблюдатель за задачей (many-to-many Task–User).</summary>
public class ProjectTaskWatcher
{
    public Guid TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
