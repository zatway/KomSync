using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviors;
using FluentValidation;

namespace Application;

public static class DependencyInjection 
{
    public static IServiceCollection ConfigureAddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); 

        services.AddAutoMapper(cfg => {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
}