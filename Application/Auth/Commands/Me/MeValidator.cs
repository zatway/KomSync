using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.Me;

public class MeValidator : AbstractValidator<MeRequest>
{
    public MeValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}