namespace Application.DTO.Projects;

public record AuthorDto(
    Guid Id,
    string Name,
    string? Email,
    bool HasAvatar
);
