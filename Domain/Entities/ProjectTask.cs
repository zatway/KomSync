using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Interfaces;
using Domain.Enums;

namespace Domain.Entities;

public class ProjectTask : IAuditable
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(500)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
    public ProjectTaskPriority Priority { get; set; } = ProjectTaskPriority.Medium;

    public DateTime? Deadline { get; set; }

    // --- Поля IAuditable (EF заполнит их сам в SaveChangesAsync) ---
    public DateTime CreatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? LastModifiedById { get; set; }

    // --- Связи ---
    public Guid ProjectId { get; set; }
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    public Guid? ParentTaskId { get; set; }
    [ForeignKey(nameof(ParentTaskId))]
    public ProjectTask? ParentTask { get; set; }
    public ICollection<ProjectTask> SubTasks { get; set; } = new List<ProjectTask>();

    [ForeignKey(nameof(CreatorId))]
    public User Creator { get; set; } = null!;

    public Guid? AssigneeId { get; set; }
    [ForeignKey(nameof(AssigneeId))]
    public User? Assignee { get; set; }

    public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    public ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();
}