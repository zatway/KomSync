using Application.DTO.Projects;
using Application.DTO.TaskComments;
using FluentValidation;

namespace Application.TaskComments.Commands.UpdateTaskComment;

public class UpdateTaskCommentValidator : AbstractValidator<UpdateTaskCommentRequest>
{
    public UpdateTaskCommentValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(200).WithMessage("Название слишком длинное");
    }
}