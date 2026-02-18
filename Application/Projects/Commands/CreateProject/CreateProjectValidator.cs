using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название проекта обязательно")
            .MaximumLength(200).WithMessage("Название не может быть длиннее 200 символов");
            
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Не указан владелец проекта");
    }
}