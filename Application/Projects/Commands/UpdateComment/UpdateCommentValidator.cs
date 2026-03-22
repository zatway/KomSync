using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.UpdateComment;

public class UpdateCommentValidator : AbstractValidator<UpdateProjectCommentRequest>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MinimumLength(1);
    }
}