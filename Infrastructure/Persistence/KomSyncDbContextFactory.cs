using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

/// <summary>
/// Фабрика для создания экземпляра контекста во время разработки (миграции).
/// Подтягивает строку подключения из WebApi/appsettings.json.
/// </summary>
public class KomSyncDbContextFactory : IDesignTimeDbContextFactory<KomSyncDbContext>
{
    public KomSyncDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KomSyncDbContext>();
        // Просто впиши строку подключения сюда на один раз
        optionsBuilder.UseNpgsql("Host=localhost;Database=komSync;Username=postgres;Password=123", 
            b => b.MigrationsAssembly("Infrastructure"));

        return new KomSyncDbContext(optionsBuilder.Options);
    }
}