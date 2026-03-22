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
            var applicationAssembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(applicationAssembly);
            });

            return services;
        }
    }
}