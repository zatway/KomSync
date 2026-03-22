namespace Application.DTO.Projects;

public record ProjectMemberDto(
    Guid Id,
    string Name,
    string? AvatarUrl,
    string? Email,
    string Role,
    DateTime JoinedAt
);