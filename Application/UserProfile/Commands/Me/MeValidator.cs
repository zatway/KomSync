using Application.DTO.UserProfile;
using FluentValidation;

namespace Application.UserProfile.Commands.Me;

public class MeValidator : AbstractValidator<MeRequest>
{
    public MeValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}