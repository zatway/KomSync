using Domain.Entities;
using Domain.Enums;

namespace Application.Common;

public static class TaskAccessRules
{
    public static bool UserCanCreateTasks(UserRole? role) =>
        role is not null and not UserRole.ReadOnly;

    /// <summary>Редактирование задачи, смена статуса, исполнитель, вложения, удаление.</summary>
    public static bool UserCanModifyTask(UserRole? role, Guid userId, ProjectTask task)
    {
        if (role is UserRole.ReadOnly) return false;
        if (role is UserRole.Admin or UserRole.Manager) return true;
        if (role is UserRole.Employee) return task.CreatorId == userId;
        return false;
    }

    public static bool UserCanAddComments(UserRole? role) =>
        role is not null and not UserRole.ReadOnly;
}
