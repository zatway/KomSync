using MediatR;

namespace Application.DTO.UserProfile;

public record MeRequest(Guid? UserId) : IRequest<UserResponse>;