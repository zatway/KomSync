using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Основной DbContext для приложения KomSync.
/// </summary>
public class KomSyncDbContext(DbContextOptions<KomSyncDbContext> options, ICurrentUserService _currentUserService)
    : DbContext(options), IKomSyncContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<TaskHistory> TaskHistories => Set<TaskHistory>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KomSyncDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? Guid.Empty;
        var now = DateTime.UtcNow;

        // Автоматическое заполнение полей аудита
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                // Ищем свойство по строковому имени, если оно есть в БД
                entry.Property("CreatedAt").CurrentValue = now;
                entry.Property("CreatorId").CurrentValue = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = now;
                entry.Property("LastModifiedById").CurrentValue = userId;
            }
        }

        var historyEntries = OnBeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);

        if (historyEntries.Any())
        {
            TaskHistories.AddRange(historyEntries);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private List<TaskHistory> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var historyEntries = new List<TaskHistory>();
        var userId = _currentUserService.UserId ?? Guid.Empty;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var taskId = (Guid)entry.Property("Id").CurrentValue!;

            if (entry.State == EntityState.Added)
            {
                historyEntries.Add(new TaskHistory
                {
                    TaskId = taskId,
                    ChangedById = userId,
                    PropertyName = "Task",
                    NewValue = "Created",
                    Action = TaskHistoryAction.Created
                });
            }
            else if (entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    if (!property.IsModified || property.Metadata.Name == "UpdatedAt")
                        continue;

                    historyEntries.Add(new TaskHistory
                    {
                        TaskId = taskId,
                        ChangedById = userId,
                        PropertyName = property.Metadata.Name,
                        OldValue = property.OriginalValue?.ToString(),
                        NewValue = property.CurrentValue?.ToString(),
                        Action = DetermineAction(property.Metadata.Name)
                    });
                }
            }
        }

        return historyEntries;
    }

    private TaskHistoryAction DetermineAction(string propertyName) => propertyName switch
    {
        "Status" => TaskHistoryAction.StatusChanged,
        "AssigneeId" => TaskHistoryAction.AssigneeChanged,
        "Priority" => TaskHistoryAction.PriorityChanged,
        _ => TaskHistoryAction.Updated
    };
}