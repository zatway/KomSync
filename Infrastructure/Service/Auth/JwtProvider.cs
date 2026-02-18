using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTO.Auth;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _config;

    public JwtProvider(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public string CreateAccessToken(Domain.Entities.User user)
    {
        var secret = _config["JwtSettings:Secret"] 
            ?? throw new InvalidOperationException("JWT Secret not found in configuration");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("fullName", user.FullName ?? string.Empty),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var expiresInMinutes = GetNumericConfig("JwtSettings:AccessTokenLifetimeMinutes", 15);

        var token = new JwtSecurityToken(
            issuer:    _config["JwtSettings:Issuer"],
            audience:  _config["JwtSettings:Audience"],
            claims:    claims,
            notBefore: DateTime.UtcNow,
            expires:   DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenResponse CreateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        var refreshToken = Convert.ToBase64String(randomBytes);

        var lifetimeDays = GetNumericConfig("JwtSettings:RefreshTokenLifetimeDays", 14);

        return new RefreshTokenResponse(refreshToken, DateTime.UtcNow.AddDays(lifetimeDays));
    }

    private int GetNumericConfig(string key, int defaultValue)
    {
        if (int.TryParse(_config[key], out var value))
            return value;
        return defaultValue;
    }
}