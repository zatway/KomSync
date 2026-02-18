using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID проекта обязателен");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(500).WithMessage("Название слишком длинное");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Описание слишком длинное");
    }
}