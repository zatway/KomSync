using Application.DTO.Attachments;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.TaskComments.Commands.UploadTaskCommentAttachments;

public class UploadTaskCommentAttachmentsHandler(
    IFmkSyncContext context,
    ICurrentUserService currentUser,
    IFileStorage storage
) : IRequestHandler<UploadTaskCommentAttachmentsCommand, IReadOnlyList<CommentAttachmentDto>>
{
    public async Task<IReadOnlyList<CommentAttachmentDto>> Handle(
        UploadTaskCommentAttachmentsCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedAccessException();

        var comment = await context.TaskComments
            .Include(c => c.Attachments)
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken);

        if (comment == null) throw new KeyNotFoundException("Comment not found");

        var created = new List<CommentAttachmentDto>();

        foreach (var file in request.Files)
        {
            if (file.Length <= 0) continue;

            await using var stream = file.OpenReadStream();
            var storedPath = await storage.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);

            var entity = new TaskCommentAttachment
            {
                Id = Guid.NewGuid(),
                TaskCommentId = comment.Id,
                FileName = file.FileName,
                ContentType = file.ContentType,
                SizeBytes = file.Length,
                StoredPath = storedPath,
                UploadedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            context.TaskCommentAttachments.Add(entity);
            comment.Attachments.Add(entity);

            created.Add(new CommentAttachmentDto(
                entity.Id,
                entity.FileName,
                entity.ContentType,
                entity.SizeBytes,
                $"/api/v1/TaskComments/attachments/{entity.Id}",
                entity.CreatedAt
            ));
        }

        await context.SaveChangesAsync(cancellationToken);
        return created;
    }
}

