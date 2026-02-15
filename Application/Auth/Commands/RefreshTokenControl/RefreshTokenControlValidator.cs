using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.RefreshTokenControl;

public class RefreshTokenControlValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenControlValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}