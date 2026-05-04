using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Комментарий к задаче.
/// </summary>
public class TaskComment
{
    [Key]
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }
    [ForeignKey(nameof(TaskId))]
    public ProjectTask Task { get; set; } = null!;

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    public string Content { get; set; } = null!;

    /// <summary>JSON-массив Guid упомянутых пользователей.</summary>
    public string? MentionsUserIdsJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Родительский комментарий (ответ в ветке).</summary>
    public Guid? ParentCommentId { get; set; }

    [ForeignKey(nameof(ParentCommentId))]
    public TaskComment? ParentComment { get; set; }

    public ICollection<TaskComment> Replies { get; set; } = new List<TaskComment>();

    public ICollection<TaskCommentAttachment> Attachments { get; set; } = new List<TaskCommentAttachment>();
}