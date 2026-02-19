using Application.DTO.Projects;
using FluentValidation;

namespace Application.Projects.Commands.DeleteProject;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectRequest>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID обязателен");
    }
}