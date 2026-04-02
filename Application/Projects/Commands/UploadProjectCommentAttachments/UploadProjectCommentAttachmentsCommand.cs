using Application.DTO.Attachments;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Projects.Commands.UploadProjectCommentAttachments;

public record UploadProjectCommentAttachmentsCommand(
    Guid CommentId,
    IReadOnlyList<IFormFile> Files
) : IRequest<IReadOnlyList<CommentAttachmentDto>>;

