using Autofac;
using EnvTracker.Application.Common;
using EnvTracker.Domain.Interfaces;
using EnvTracker.Infrastructure.Authentication.OptionsSetup;
using EnvTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace EnvTracker.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IRepository, DapperRepository>();
        }

        public static void RegisterInfrastructureServices(this ContainerBuilder builder)
        {
            builder.RegisterType<JwtTokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<DapperRepository>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
