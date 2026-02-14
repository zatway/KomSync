using Application.Interfaces;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Auth;

namespace Application.Auth.Commands.Register;

public class RegisterHandler(
    IKomSyncContext context,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IMapper mapper) : IRequestHandler<RegisterCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Проверка уникальности
        if (await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new Exception("Пользователь с таким Email уже существует");

        // 2. Создание пользователя
        var user = mapper.Map<User>(request);
        user.PasswordHash = passwordHasher.Hash(request.Password);

        context.Users.Add(user);
        
        // 3. Генерация токенов (логику GenerateTokenPair можно вынести в хелпер или оставить здесь)
        var accessToken = jwtProvider.CreateAccessToken(user);
        var refreshTokenValue = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // Упростил для примера

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TokenResponse(accessToken, refreshTokenValue);
    }
}