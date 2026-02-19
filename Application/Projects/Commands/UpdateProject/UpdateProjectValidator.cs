using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID проекта обязателен");

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Название слишком длинное");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Описание слишком длинное");
    }
}