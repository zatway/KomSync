namespace Application.DTO.Auth;

public record TokenResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiredAt, DateTime RefreshTokenExpiredAt);