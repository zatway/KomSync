namespace Application.DTO.Projects;

public record PermissionsDto(
    bool CanEdit,
    bool CanDelete,
    bool CanManageMembers,
    bool CanViewHistory,
    bool CanManageTaskColumns,
    bool CanCreateTasks
);
