using Application.DTO.Projects;
using Application.DTO.TaskComments;
using FluentValidation;

namespace Application.TaskComments.Commands.DeleteProject;

public class DeleteTaskCommentValidator : AbstractValidator<DeleteTaskCommentRequest>
{
    public DeleteTaskCommentValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID обязателен");
    }
}