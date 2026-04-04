using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.AssignUser;

public class AssignUserValidator : AbstractValidator<AssignUserRequest>
{
    public AssignUserValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("ID задачи обязателен");
    }
}