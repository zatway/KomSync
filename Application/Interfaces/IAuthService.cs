using Application.DTO.Auth;

namespace Application.Interfaces;

/// <summary>
///     Интерфейс для сервиса аутентификации
/// </summary>
public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(MeRequest request);
    Task<TokenResponse?> RegisterAsync(RegisterRequest request);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
}