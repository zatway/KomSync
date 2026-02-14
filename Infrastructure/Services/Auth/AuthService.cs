using System.Security.Cryptography;
using Application.Interfaces;
using Domain.Entities;
using Application.DTO.Auth;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Auth;

public class AuthService(
    IKomSyncContext context,
    IJwtProvider jwtProvider,
    IMapper mapper,
    IPasswordHasher passwordHasher) : IAuthService
{
    public async Task<TokenResponse?> LoginAsync(LoginRequest request)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        return await GenerateTokenPairAsync(user);
    }

    public async Task<TokenResponse?> RegisterAsync(RegisterRequest request)
    {
        // 1. Проверяем, не занят ли Email
        var exists = await context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists) 
            throw new Exception("Пользователь с таким Email уже существует");

        // 2. Маппим DTO в сущность User
        var user = mapper.Map<User>(request);

        // 3. Хешируем пароль и сохраняем
        user.PasswordHash = passwordHasher.Hash(request.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // 4. Генерируем токены для автоматического входа после регистрации
        return await GenerateTokenPairAsync(user);
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
    {
        // Ищем пользователя, у которого есть этот активный токен
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken && !t.IsRevoked));

        if (user == null) return null;

        var existingToken = user.RefreshTokens.First(t => t.Token == refreshToken);

        // Проверяем срок годности
        if (existingToken.ExpiresAt < DateTime.UtcNow)
        {
            context.RefreshTokens.Remove(existingToken);
            await context.SaveChangesAsync();
            return null;
        }

        // Удаляем старый токен (Rotation policy: один раз использовал — до свидания)
        context.RefreshTokens.Remove(existingToken);
        
        return await GenerateTokenPairAsync(user);
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken);

        if (token == null) return false;

        context.RefreshTokens.Remove(token);
        return await context.SaveChangesAsync() > 0;
    }

    // --- Helper Function ---
    private async Task<TokenResponse> GenerateTokenPairAsync(User user)
    {
        var accessToken = jwtProvider.CreateAccessToken(user);
        
        // Генерация криптографически стойкого Refresh Token
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshTokenValue = Convert.ToBase64String(randomNumber);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,
            IsRevoked = false
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return new TokenResponse(accessToken, refreshTokenValue);
    }
}