using Domain.Entities;

namespace Application.DTO.Auth;

public record TokenResponse(string AccessToken, RefreshToken RefreshToken);