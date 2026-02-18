using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusValidator : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("ID проекта обязателен");

        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("Статус не может быть пустым");
    }
}