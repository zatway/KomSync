using Application.DTO.Attachments;

namespace Application.DTO.Projects;

public record ProjectCommentDto(
    Guid Id,
    Guid ProjectId,
    string Content,
    AuthorDto Author,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    Guid? ParentId
)
{
    public List<ProjectCommentDto> Replies { get; set; } = new List<ProjectCommentDto>();
    public IReadOnlyList<CommentAttachmentDto> Attachments { get; set; } = Array.Empty<CommentAttachmentDto>();
}