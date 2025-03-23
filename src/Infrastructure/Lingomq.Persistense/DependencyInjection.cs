using System.Reflection;
using AutoMapper.Extensions.ExpressionMapping;
using LingoMQ.Core.Application.Features.Users;
using LingoMQ.Core.Application.Features.Words;
using LingoMQ.Core.Domain.Users;
using LingoMQ.Core.Domain.Words;
using LingoMQ.Infrastructure.Persistense.EntityFramework;
using LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Users;
using LingoMQ.Infrastructure.Persistense.EntityFramework.Repositories.Words;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistense(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
            cfg.AddExpressionMapping();
        });
        services.AddUsersEfContext(configuration);
        services.AddWordsEfContext(configuration);
        services.AddUsersPersistense();
        services.AddWordsPersistense();
        services.AddEfUnitOfWork();

        return services;
    }

    public static IServiceCollection AddEfUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUsersUnitOfWork, EfUsersUnitOfWork>();
        services.AddScoped<IWordsUnitOfWork, EfWordsUnitOfWork>();

        return services;
    }

    public static IServiceCollection AddWordsPersistense(this IServiceCollection services)
    {
        services.AddScoped<IWordInfoRepository, WordInfoRepository>();
        services.AddScoped<IUserWordRepository, UserWordRepository>();

        return services;
    }

    public static IServiceCollection AddUsersPersistense(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();

        return services;
    }

    public static IServiceCollection AddWordsEfContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        switch (configuration["ConnectionString:Words:ProviderName"])
        {
            default:
                services.AddDbContext<WordsDbContext>(o =>
                {
                    o.UseNpgsql(configuration["ConnectionString:Words:Value"]);
                    o.UseSnakeCaseNamingConvention();
                    o.UseLazyLoadingProxies();
                });
                break;
        }
        return services;
    }

    public static IServiceCollection AddUsersEfContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        switch (configuration["ConnectionString:Users:ProviderName"])
        {
            default:
                services.AddDbContext<UsersDbContext>(o =>
                {
                    o.UseNpgsql(configuration["ConnectionString:Users:Value"]);
                    o.UseSnakeCaseNamingConvention();
                    o.UseLazyLoadingProxies();
                });
                break;
        }
        return services;
    }

    public static void UsersInitializeDatabaseByEf(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            context.Database.Migrate();
        }
    }

    public static void WordsInitializeDatabaseByEf(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WordsDbContext>();
            context.Database.Migrate();
        }
    }
}
