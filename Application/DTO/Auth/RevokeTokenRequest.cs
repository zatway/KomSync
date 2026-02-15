using MediatR;

namespace Application.DTO.Auth;

public record RevokeTokenRequest(string RefreshToken) : IRequest<bool>;