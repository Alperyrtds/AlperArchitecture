using Domain.Abstractions;
using Domain.Models.MssqlContext;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(
            factory => factory.GetRequiredService<AlperProjectContext>());
        return services;
    }
}
