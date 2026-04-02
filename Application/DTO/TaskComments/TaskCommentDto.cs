using Application.DTO.Attachments;

namespace Application.DTO.TaskComment;

public record TaskCommentDto
{
    public TaskCommentDto() { }

    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string AuthorName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IReadOnlyList<CommentAttachmentDto> Attachments { get; set; } = Array.Empty<CommentAttachmentDto>();
}