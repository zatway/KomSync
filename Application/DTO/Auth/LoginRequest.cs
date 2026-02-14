namespace Application.DTO.Auth;

public record LoginRequest(string Email, string Password, string? ExternalProvider);