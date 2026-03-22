using MediatR;

namespace Application.DTO.UserProfile;

public record MeAvatarRequest(Guid? UserId) : IRequest<AvatarResult>;
