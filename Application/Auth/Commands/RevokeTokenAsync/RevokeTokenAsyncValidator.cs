using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.RevokeTokenAsync;

public class RevokeTokenAsyncValidator : AbstractValidator<RevokeTokenRequest>
{
    public RevokeTokenAsyncValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}