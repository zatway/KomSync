using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Пароль слишком короткий");
    }
}