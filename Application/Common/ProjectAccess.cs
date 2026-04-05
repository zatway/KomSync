using Domain.Entities;
using Domain.Enums;

namespace Application.Common;

public static class ProjectAccessRules
{
    public static bool CanViewAllProjects(UserRole? role) =>
        role is UserRole.Admin or UserRole.Manager;

    public static bool UserCanViewProject(UserRole? role, Guid userId, Project project) =>
        CanViewAllProjects(role) || project.OwnerId == userId || project.Members.Any(m => m.Id == userId);
}
