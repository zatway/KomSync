using Application.DTO.Task;
using Application.DTO.Tasks;
using FluentValidation;

namespace Application.TaskComments.Commands.AddTaskComment;

public class AddTaskCommentValidator : AbstractValidator<AddTaskCommentRequest>
{
    public AddTaskCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(500);
        RuleFor(x => x.TaskId).NotEmpty();
    }
}