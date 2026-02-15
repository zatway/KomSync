using Application.DTO.Auth;
using FluentValidation;

namespace Application.Auth.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Role).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Пароль слишком короткий");    
    }
}