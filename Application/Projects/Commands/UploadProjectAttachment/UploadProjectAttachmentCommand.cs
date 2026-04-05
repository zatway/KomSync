using Application.DTO.Attachments;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Projects.Commands.UploadProjectAttachment;

public record UploadProjectAttachmentCommand(Guid ProjectId, IReadOnlyList<IFormFile> Files)
    : IRequest<IReadOnlyList<FileAttachmentDto>>;
