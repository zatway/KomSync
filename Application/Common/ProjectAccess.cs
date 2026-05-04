using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public static class ProjectAccessRules
{
    public static bool CanViewAllProjects(UserRole? role) =>
        role is UserRole.Admin or UserRole.Manager;

    /// <summary>Админ и менеджер: создание/удаление/редактирование проекта и колонок статусов.</summary>
    public static bool UserCanManageProjectsAndColumns(UserRole? role) =>
        role is UserRole.Admin or UserRole.Manager;

    /// <summary>
    /// Просмотр проекта: админ/менеджер — все; сотрудник/читатель — проекты своего отдела или где пользователь владелец;
    /// иначе (устаревшее) — участник или владелец.
    /// </summary>
    public static bool UserCanViewProject(UserRole? role, Guid userId, Project project, Guid? userDepartmentId)
    {
        if (CanViewAllProjects(role))
            return true;

        if (project.OwnerId == userId)
            return true;

        if (role is UserRole.Employee or UserRole.ReadOnly)
        {
            if (userDepartmentId.HasValue && project.DepartmentId == userDepartmentId.Value)
                return true;
            return false;
        }

        return project.Members.Any(m => m.Id == userId);
    }

    public static IQueryable<Project> WhereUserCanSeeProject(
        this IQueryable<Project> query,
        UserRole? role,
        Guid userId,
        Guid? userDepartmentId)
    {
        if (CanViewAllProjects(role))
            return query;

        if (role is UserRole.Employee or UserRole.ReadOnly)
        {
            if (userDepartmentId.HasValue)
                return query.Where(p => p.DepartmentId == userDepartmentId.Value || p.OwnerId == userId);
            return query.Where(p => p.OwnerId == userId);
        }

        return query.Where(p => p.OwnerId == userId || p.Members.Any(m => m.Id == userId));
    }
}
