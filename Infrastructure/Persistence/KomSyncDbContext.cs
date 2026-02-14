using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Основной DbContext для приложения KomSync.
/// </summary>
public class KomSyncDbContext(DbContextOptions<KomSyncDbContext> options) 
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
        
        // Применяем все конфигурации (IEntityTypeConfiguration) из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KomSyncDbContext).Assembly);
    }
}