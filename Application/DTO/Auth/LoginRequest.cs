using MediatR;

namespace Application.DTO.Auth;

public record LoginRequest(string Email, string Password) : IRequest<TokenResponse>;