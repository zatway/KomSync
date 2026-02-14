using Application.DTO.Auth;
using MediatR;

namespace Application.Auth.Commands.Register;

public record RegisterCommand(string FullName, string Email, string Password) : IRequest<TokenResponse>;