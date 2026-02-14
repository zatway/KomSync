using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Задача в трекере.
/// </summary>
public class ProjectTask
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(500)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
    public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Medium;

    public DateTime? Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // --- Связи ---

    /// <summary> ID проекта. </summary>
    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    /// <summary> Родительская задача (для подзадач). </summary>
    public Guid? ParentTaskId { get; set; }
    [ForeignKey(nameof(ParentTaskId))]
    public ProjectTask? ParentTask { get; set; }
    public ICollection<ProjectTask> SubTasks { get; set; } = new List<ProjectTask>();

    /// <summary> Создатель задачи. </summary>
    public Guid CreatorId { get; set; }
    [ForeignKey(nameof(CreatorId))]
    public User Creator { get; set; } = null!;

    /// <summary> Исполнитель задачи. </summary>
    public Guid? AssigneeId { get; set; }
    [ForeignKey(nameof(AssigneeId))]
    public User? Assignee { get; set; }

    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    public ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();
}