using Domain.Enums;

namespace Application.DTO.UserProfile;

public record UserResponse(string FullName,
    string Email, UserRole Role, string DepartmentName,
    string PositionName);