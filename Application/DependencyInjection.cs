using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Alper.Application.Behaviors;
using Alper.Infrastructure.Models;
using Alper.Repository.Abstractions;
using Alper.Repository.Models;
using Alper.Repository.Repositories;
using MediatR;

namespace Alper.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration inConfig)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddScoped<IProjectRepository<TblUsers>, ProjectRepository<TblUsers>>();

        return services;
    }
}