using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusValidator : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.NewStatusColumnId).NotEmpty();
    }
}
