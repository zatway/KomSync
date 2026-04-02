namespace Application.DTO.Attachments;

public record CommentAttachmentDto(
    Guid Id,
    string FileName,
    string? ContentType,
    long SizeBytes,
    string DownloadUrl,
    DateTime CreatedAt
);

