using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class FmkSyncDbContextFactory : IDesignTimeDbContextFactory<FmkSyncDbContext>
{
    public FmkSyncDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FmkSyncDbContext>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApi"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var cs = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");

        optionsBuilder.UseNpgsql(cs, b => b.MigrationsAssembly("Infrastructure"));

        // Создаем заглушку сервиса пользователя для миграций
        return new FmkSyncDbContext(optionsBuilder.Options, new DesignTimeUserService());
    }
}

// Простая заглушка внутри того же файла
public class DesignTimeUserService : ICurrentUserService
{
    public Guid? UserId => Guid.Empty;
    public Domain.Enums.UserRole? Role => null;
}