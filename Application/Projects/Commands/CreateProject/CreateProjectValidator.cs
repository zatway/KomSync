using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Key).NotEmpty().Matches("^[A-Z0-9_-]{2,10}$");
    }
}