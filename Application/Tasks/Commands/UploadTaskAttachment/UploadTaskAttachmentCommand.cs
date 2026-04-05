using Application.DTO.Attachments;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Tasks.Commands.UploadTaskAttachment;

public record UploadTaskAttachmentCommand(Guid TaskId, IReadOnlyList<IFormFile> Files)
    : IRequest<IReadOnlyList<FileAttachmentDto>>;
