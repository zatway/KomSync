using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.Login;

public class LoginHandler(
    IKomSyncContext context,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider
    ) : IRequestHandler<LoginRequest, TokenResponse>
{
    public async Task<TokenResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new Exception("Пользователя с таким email не существует или неверный пароль");
        
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