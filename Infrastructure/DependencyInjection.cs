using Application.Interfaces;
using Application.Mapping;
using Infrastructure.Persistence;
using Infrastructure.Service.Auth;
using Infrastructure.Service.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureAddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KomSyncDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(KomSyncDbContext).Assembly.FullName)));
            
        services.AddScoped<IKomSyncContext>(provider => provider.GetRequiredService<KomSyncDbContext>());

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        // Исправленная регистрация AutoMapper
        services.AddAutoMapper(cfg => {}, typeof(AuthMappingProfile).Assembly);

        return services;
    }
}