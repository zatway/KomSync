using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Auth;

public class JwtProvider(IConfiguration config) : IJwtProvider
{
    public string CreateAccessToken(User user)
    {
        var secret = config["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Добавляем полезную нагрузку (Claims)
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("fullName", user.FullName),
            new(ClaimTypes.Role, user.Role.ToString()) // Роль из твоего Enum
        };

        var token = new JwtSecurityToken(
            issuer: config["JwtSettings:Issuer"],
            audience: config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Access token живет недолго
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}