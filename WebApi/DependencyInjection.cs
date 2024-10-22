using Alper.Application.Commands.UserCmds;
using Application.Commands.UserCmds;
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
