using Application.DTO.Auth;
using Application.DTO.UserProfile;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserProfile.Queries.MeAvatar;

public class MeAvatarHandler(
    IKomSyncContext context
) : IRequestHandler<MeAvatarRequest, AvatarResult>
{
    public async Task<AvatarResult> Handle(MeAvatarRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId == null)
            throw new UnauthorizedAccessException("UserId is null");

        var user = await context.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => new { u.Avatar })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null || user.Avatar == null)
            throw new Exception("Avatar not found");

        return new AvatarResult(user.Avatar, "image/png"); // MIME можно определять динамически
    }
}