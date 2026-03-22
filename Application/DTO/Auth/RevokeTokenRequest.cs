using MediatR;

namespace Application.DTO.Auth;

public record RevokeTokenRequest(Guid UserId) : IRequest<bool>;