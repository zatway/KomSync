using Application.Interfaces;
using Domain.Entities;
namespace Application.Projects.ProjectTaskStatusColumns;

public static class ProjectTaskStatusColumnSeeder
{
    public static void SeedDefaultsForProject(IFmkSyncContext context, Guid projectId)
    {
        // Три обязательных этапа для корректной аналитики: открытые / в работе / закрытые.
        var defs = new (string Name, int Order, byte Kind, bool Done, bool Blocked)[]
        {
            ("Открыта", 0, 0, false, false),
            ("В работе", 1, 1, false, false),
            ("Закрыто", 2, 3, true, false),
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
