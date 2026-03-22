namespace Application.DTO.Projects;


public record ProjectHistoryEntryDto(
    Guid Id,
    string Field,
    string OldValue,
    string NewValue,
    ChangedByDto ChangedBy,
    DateTimeOffset ChangedAt
);