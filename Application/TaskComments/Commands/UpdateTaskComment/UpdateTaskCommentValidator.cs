using Application.DTO.Projects;
using Application.DTO.TaskComments;
using FluentValidation;

namespace Application.TaskComments.Commands.UpdateTaskComment;

public class UpdateTaskCommentValidator : AbstractValidator<UpdateTaskCommentRequest>
{
    public UpdateTaskCommentValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Текст не более 500 символов");
    }
}