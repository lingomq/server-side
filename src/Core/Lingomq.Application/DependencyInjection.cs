using System.Reflection;
using FluentValidation;
using LingoMQ.Core.Application.Behaviours;
using LingoMQ.Core.Application.Features.Auth.Jwt;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        });

        return services;
    }

    public static IServiceCollection AddWordsFeatures(this IServiceCollection services) => services;

    public static IServiceCollection AddUsersFeatures(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOptions();
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtConfiguration"));
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
