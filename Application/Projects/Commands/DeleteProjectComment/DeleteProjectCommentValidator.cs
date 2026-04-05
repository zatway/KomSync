using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.DeleteProjectComment;

public class DeleteProjectCommentValidator : AbstractValidator<DeleteProjectCommentRequest>
{
    public DeleteProjectCommentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
