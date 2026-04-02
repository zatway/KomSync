namespace Application.DTO.Tasks;

public record TaskStatusColumnDto(
    Guid Id,
    string Name,
    int SortOrder,
    string? ColorHex,
    byte SemanticKind,
    bool IsDoneColumn,
    bool IsBlockedColumn
);
