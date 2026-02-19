namespace Application.DTO.TaskComment;

public record TaskCommentDto
{
    public Guid Id { get; set; }
    public TaskCommentDto() { }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}