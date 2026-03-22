using Application.Auth.AccessToken;
using Application.DTO.Auth;
using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtProvider
{
    public AccessTokenResult CreateAccessToken(User user);
    public RefreshTokenResponse CreateRefreshToken();
}