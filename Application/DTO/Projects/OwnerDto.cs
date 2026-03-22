using Domain.Enums;

namespace Application.DTO.Projects;

public record OwnerDto(
    Guid Id,
    string Name,
    string? Email,
    UserRole Role
);