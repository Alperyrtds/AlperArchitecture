using Alper.Infrastructure.Models.MssqlContext;

using Alper.Repository.Abstractions;
using Alper.Repository.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alper.Repository;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration inConfig)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = inConfig.GetConnectionString("RedisConnection");
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = inConfig.GetConnectionString("RedisConnection");
        });
        services.AddScoped<IUnitOfWork>(
            factory => factory.GetRequiredService<AlperProjectContext>());
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
}

