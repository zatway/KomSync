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
    public DbSet<ApplicationForRegistration> ApplicationForRegistrations => Set<ApplicationForRegistration>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<ProjectComment> ProjectComments => Set<ProjectComment>();
    public DbSet<ProjectHistory> ProjectHistories => Set<ProjectHistory>();
    public DbSet<Tag> Tags => Set<Tag>();
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
        var ignored = new[] { "UpdatedAt", "CreatedAt" };
        
        var historyEntries = new List<ProjectHistory>();

        var entries = ChangeTracker.Entries<Project>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var projectId = entry.Entity.Id;

            foreach (var property in entry.Properties)
            {
                if (ignored.Contains(property.Metadata.Name) || !property.IsModified)
                    continue;

                var oldValue = property.OriginalValue?.ToString();
                var newValue = property.CurrentValue?.ToString();

                if (oldValue == newValue) continue;

                historyEntries.Add(new ProjectHistory
                {
                    ProjectId = projectId,
                    Field = property.Metadata.Name,
                    OldValue = oldValue ?? string.Empty,
                    NewValue = newValue ?? string.Empty,
                    ChangedById = _currentUserService.UserId
                });
            }
        }

        if (historyEntries.Count > 0)
            await ProjectHistories.AddRangeAsync(historyEntries, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

}