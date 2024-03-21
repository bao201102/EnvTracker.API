using Autofac;
using EnvTracker.Application.Services.Implements.STA;
using EnvTracker.Application.Services.Implements.USR;
using EnvTracker.Application.Services.Interfaces.STA;
using EnvTracker.Application.Services.Interfaces.USR;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace EnvTracker.Application.Configuration
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<ISensorService, SensorService>();
        }

        public static void RegisterApplicationServices(this ContainerBuilder builder)
        {
            // register for services
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
        }
    }
}
