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
        // Ищем пользователя, у которого есть этот активный токен
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken && !t.IsRevoked), cancellationToken);

        if (user == null) throw new Exception("Не найден пользователь с таким токеном");
        
        var existingToken = user.RefreshTokens.First(t => t.Token == request.RefreshToken);

        // Проверяем срок годности
        if (existingToken.ExpiresAt < DateTime.UtcNow)
        {
            context.RefreshTokens.Remove(existingToken);
            await context.SaveChangesAsync(cancellationToken);
            throw new Exception("Токен истек");
        }
        
        context.RefreshTokens.Remove(existingToken);
        
        var accessToken = jwtProvider.CreateAccessToken(user);
        var refreshTokenValue = jwtProvider.CreateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue.RefreshToken,
            ExpiresAt = refreshTokenValue.ExpiredTime,
            UserId = user.Id
        };

        return new TokenResponse(accessToken, refreshToken);
    }
}