using MediatR;

namespace Application.DTO.Auth;

public record MeRequest(Guid? UserId) : IRequest<UserResponse>;