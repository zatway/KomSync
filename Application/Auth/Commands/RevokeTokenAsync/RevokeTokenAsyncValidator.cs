using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.RevokeTokenAsync;

public class RevokeTokenAsyncValidator : AbstractValidator<RefreshTokenRequest>
{
    public RevokeTokenAsyncValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}