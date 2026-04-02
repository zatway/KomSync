using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID задачи обязателен");

        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(500)
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .MaximumLength(4000)
            .When(x => x.Description != null);
    }
}
