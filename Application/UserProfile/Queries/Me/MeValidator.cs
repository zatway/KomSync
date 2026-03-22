using Application.DTO.UserProfile;
using FluentValidation;

namespace Application.UserProfile.Queries.Me;

public class MeValidator : AbstractValidator<MeRequest>
{
    public MeValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}