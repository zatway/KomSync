using Application.DTO.Attachments;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.TaskComments.Commands.UploadTaskCommentAttachments;

public record UploadTaskCommentAttachmentsCommand(
    Guid CommentId,
    IReadOnlyList<IFormFile> Files
) : IRequest<IReadOnlyList<CommentAttachmentDto>>;

