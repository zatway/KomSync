using Domain.Enums;

namespace Application.DTO.Auth;

public record UserResponse(byte[]? Avatar,   string FullName,
    string Email, UserRole Role, string DepartmentName,
    string PositionName);