using Domain.Enums;

namespace Application.DTO.Auth;

public record RegisterRequest(string FullName, string Email, string Password, UserRole Role, string? Position, string? Department, string? ExternalProvider);