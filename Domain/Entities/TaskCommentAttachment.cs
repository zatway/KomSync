using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class TaskCommentAttachment
{
    public Guid Id { get; set; }

    public Guid TaskCommentId { get; set; }
    [ForeignKey(nameof(TaskCommentId))]
    public TaskComment TaskComment { get; set; } = null!;

    [Required, MaxLength(260)]
    public string FileName { get; set; } = null!;

    [MaxLength(120)]
    public string? ContentType { get; set; }

    public long SizeBytes { get; set; }

    /// <summary>Относительный путь в хранилище (uploads/...).</summary>
    [Required, MaxLength(500)]
    public string StoredPath { get; set; } = null!;

    public Guid UploadedByUserId { get; set; }
    [ForeignKey(nameof(UploadedByUserId))]
    public User UploadedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
