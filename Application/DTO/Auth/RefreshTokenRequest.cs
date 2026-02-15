using MediatR;

namespace Application.DTO.Auth;

public record RefreshTokenRequest(string RefreshToken) : IRequest<TokenResponse>;