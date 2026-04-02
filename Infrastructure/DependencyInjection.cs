using Application.Interfaces;
using Application.Mapping;
using Infrastructure.Persistence;
using Application.Interfaces;
using Infrastructure.Service.Auth;
using Infrastructure.Service.Notifications;
using Infrastructure.Service.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

        services.Configure<SmtpEmailSettings>(configuration.GetSection("SmtpEmail"));
        services.AddSingleton<IEmailSender>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<SmtpEmailSettings>>().Value;
            if (settings.Enabled)
                return new SmtpEmailSender(sp.GetRequiredService<IOptions<SmtpEmailSettings>>());
            return new LoggingEmailSender(sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LoggingEmailSender>>());
        });
        
        // Исправленная регистрация AutoMapper
        services.AddAutoMapper(cfg => {}, typeof(AuthMappingProfile).Assembly);

        return services;
    }
}