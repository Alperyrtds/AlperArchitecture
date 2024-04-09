using Domain.Abstractions;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration inConfig)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IProjectRepository<TblEmployee>, ProjectRepository<TblEmployee>>();

        return services;
    }
}