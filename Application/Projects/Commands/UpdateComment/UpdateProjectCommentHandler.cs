using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.UpdateComment
{
    public class UpdateProjectCommentHandler(IKomSyncContext context, ICurrentUserService currentUser)
        : IRequestHandler<UpdateProjectCommentRequest, bool>
    {
        public async Task<bool> Handle(UpdateProjectCommentRequest request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId == null)
                throw new UnauthorizedAccessException("User not authorized");

            var comment = await context.ProjectComments
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (comment == null)
                throw new Exception("Comment not found");

            if (comment.AuthorId != currentUser.UserId)
                throw new UnauthorizedAccessException("Cannot edit another user's comment");

            comment.Content = request.Content;
            comment.UpdateTimestamp();

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}