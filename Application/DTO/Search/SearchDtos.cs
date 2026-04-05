namespace Application.DTO.Search;

public record SearchHitDto(string Kind, Guid Id, string Title, string? Subtitle, Guid? ProjectId, string? ProjectKey);
