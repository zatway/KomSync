using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviors;
using FluentValidation;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureAddApplication(this IServiceCollection services)
    {
        // Регистрируем MediatR
        // Он автоматически найдет все Commands и Query Handler-ы в этом проекте
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Регистрируем AutoMapper (профили маппинга)
        services.AddAutoMapper(cfg => {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
}