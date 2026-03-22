using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.RefreshTokenControl;

public class RefreshTokenControlHandler(
    IKomSyncContext context,
    IJwtProvider jwtProvider
) : IRequestHandler<RefreshTokenRequest, TokenResponse>
{
    public async Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var existingToken = await context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                    t.Token == request.RefreshToken &&
                    !t.IsRevoked,
                cancellationToken);

        if (existingToken == null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (existingToken.ExpiresAt < DateTime.UtcNow)
        {
            existingToken.IsRevoked = true;
            await context.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedAccessException("Refresh token expired");
        }

        var user = existingToken.User;

        existingToken.IsRevoked = true;

        var accessToken = jwtProvider.CreateAccessToken(user);
        var newRefreshTokenData = jwtProvider.CreateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            Token = newRefreshTokenData.RefreshToken,
            ExpiresAt = newRefreshTokenData.ExpiredTime,
            UserId = user.Id
        };

        await context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TokenResponse(
            accessToken.Token,
            newRefreshToken.Token,
            accessToken.ExpiresAt,
            newRefreshToken.ExpiresAt
        );
    }
}