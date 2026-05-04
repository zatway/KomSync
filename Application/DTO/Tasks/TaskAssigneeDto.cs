namespace Application.DTO.Tasks;

public record TaskAssigneeDto(Guid Id, string Name, string? AvatarUrl, bool HasAvatar);
