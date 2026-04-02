using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.ProjectTaskStatusColumnId).NotEmpty();
        RuleFor(x => x.Priority).IsInEnum();
    }
}
