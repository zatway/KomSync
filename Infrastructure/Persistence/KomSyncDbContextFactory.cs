using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

/// <summary>
/// Фабрика для создания экземпляров <see cref="KomSyncDbContext" />.
/// Используется инструментами EF Core (например, Add-Migration) для создания контекста без запуска приложения.
/// </summary>
public class KomSyncDbContextFactory : IDesignTimeDbContextFactory<KomSyncDbContext>
{
    /// <summary>
    /// Создает новый экземпляр контекста базы данных.
    /// </summary>
    /// <param name="args">Аргументы командной строки (не используются).</param>
    /// <returns>Новый экземпляр <see cref="KomSyncDbContext" />.</returns>
    public KomSyncDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KomSyncDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=komsync;Username=postgres;Password=postgres", 
            builder => builder.MigrationsAssembly("Infrastructure"));

        return new KomSyncDbContext(optionsBuilder.Options);
    }
}