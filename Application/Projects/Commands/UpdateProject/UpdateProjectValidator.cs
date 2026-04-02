using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.UpdateProject;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Key)
            .Matches("^[A-Z0-9_-]{2,10}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Key));

        RuleFor(x => x.Color)
            .Matches("^#([A-Fa-f0-9]{6})$")
            .When(x => !string.IsNullOrWhiteSpace(x.Color))
            .WithMessage("Color must be in format #RRGGBB");
    }
}