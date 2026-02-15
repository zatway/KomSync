namespace Application.DTO.Auth;

public record RefreshTokenResponse(string RefreshToken, DateTime ExpiredTime);