using Application.DTO.TaskComments;
using FluentValidation;

namespace Application.TaskComments.Commands.DeleteTaskComment;

public class DeleteTaskCommentValidator : AbstractValidator<DeleteTaskCommentRequest>
{
    public DeleteTaskCommentValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID обязателен");
    }
}