using Application.Interfaces;
using Domain.Entities;
namespace Application.Projects.ProjectTaskStatusColumns;

public static class ProjectTaskStatusColumnSeeder
{
    public static void SeedDefaultsForProject(IKomSyncContext context, Guid projectId)
    {
        var defs = new (string Name, int Order, byte Kind, bool Done, bool Blocked)[]
        {
            ("К выполнению", 0, 0, false, false),
            ("В работе", 1, 1, false, false),
            ("На проверке", 2, 2, false, false),
            ("Готово", 3, 3, true, false),
            ("Заблокировано", 4, 4, false, true),
        };

        foreach (var d in defs)
        {
            context.ProjectTaskStatusColumns.Add(new ProjectTaskStatusColumn
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Name = d.Name,
                SortOrder = d.Order,
                SemanticKind = d.Kind,
                IsDoneColumn = d.Done,
                IsBlockedColumn = d.Blocked,
            });
        }
    }
}
