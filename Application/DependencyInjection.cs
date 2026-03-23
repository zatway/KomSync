using System.Reflection;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Behaviors;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureAddApplication(this IServiceCollection services)
        {
            // Находим все сборки приложения
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && a.GetName().Name!.StartsWith("Application"))
                .ToArray();

            // === MediatR: автоматическая регистрация всех IRequestHandler<>
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Pipeline Behavior
            });

            // === FluentValidation: автоматическая регистрация всех IValidator<>
            foreach (var assembly in assemblies)
            {
                services.AddValidatorsFromAssembly(assembly);
            }

            // === AutoMapper: автоматическая регистрация всех Mapping Profiles
            services.AddAutoMapper(cfg =>
            {
                foreach (var assembly in assemblies)
                    cfg.AddMaps(assembly);
            });

            return services;
        }
    }
}