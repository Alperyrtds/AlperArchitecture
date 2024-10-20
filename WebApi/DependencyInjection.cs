using Application.Commands.UserCmds;
using Domain.Abstractions;
using Domain.Models;
using Domain.Repositories;
using FluentValidation;
using WebApi.Filters;
using WebApi.Validators.Employees;

namespace Alper.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration inConfig)
        {

            services.AddScoped(typeof(AlperCmdFilter<CreateUserCmd, CreateUserVld>));

            return services;
        }
    }
}
