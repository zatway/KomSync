namespace Application.DTO.Attachments;

public record FileAttachmentDto(
    Guid Id,
    string FileName,
    string? ContentType,
    long SizeBytes,
    string DownloadUrl,
    DateTime CreatedAt
);
