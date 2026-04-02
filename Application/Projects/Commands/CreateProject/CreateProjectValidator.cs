using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);

        RuleFor(x => x.Key)
            .NotEmpty()
            .Matches("^[A-Z0-9_-]{2,10}$");

        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.Color)
            .Matches("^#([A-Fa-f0-9]{6})$")
            .When(x => !string.IsNullOrWhiteSpace(x.Color))
            .WithMessage("Color must be in format #RRGGBB");
    }
}