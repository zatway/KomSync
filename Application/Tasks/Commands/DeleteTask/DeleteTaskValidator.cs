using Application.DTO.Projects;
using Application.DTO.Tasks;
using FluentValidation;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskValidator : AbstractValidator<DeleteTaskRequest>
{
    public DeleteTaskValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID обязателен");
    }
}