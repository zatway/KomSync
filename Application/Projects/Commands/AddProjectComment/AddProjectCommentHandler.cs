using Application.DTO.Projects;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Projects.Commands.AddProjectComment
{
    public class AddProjectCommentHandler(IKomSyncContext context, ICurrentUserService currentUser)
        : IRequestHandler<CreateProjectCommentRequest, ProjectCommentDto>
    {
        public async Task<ProjectCommentDto> Handle(CreateProjectCommentRequest request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId == null)
                throw new UnauthorizedAccessException("User not authorized");

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            var comment = new ProjectComment
            {
                ProjectId = request.ProjectId,
                Content = request.Content,
                AuthorId = user.Id,
                ParentId = request.ParentId
            };

            await context.ProjectComments.AddAsync(comment, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new ProjectCommentDto(
                comment.Id,
                comment.ProjectId,
                comment.Content,
                new AuthorDto(comment.AuthorId, comment.Author.FullName, comment.Author.Email),
                comment.CreatedAt,
                comment.UpdatedAt,
                comment.ParentId
            );
        }
    }
}