using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.AddProjectComment;

public class AddProjectCommentValidator : AbstractValidator<CreateProjectCommentRequest>
{
    public AddProjectCommentValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MinimumLength(1);
    }
}
