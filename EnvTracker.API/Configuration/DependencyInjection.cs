using Autofac;
using EnvTracker.Application.Configuration;
using EnvTracker.Domain;
using EnvTracker.Infrastructure.Configuration;
using Npgsql;
using System.Data;

namespace EnvTracker.API.Configuration
{
    public static class DependencyInjection
    {
        // register services for Microsoft DI
        public static void AddServices(this IServiceCollection services)
        {
            services.AddInfrastructureServices();
            services.AddApplicationServices();
        }

        // register services for autofac
        public static void RegisterServices(this ContainerBuilder builder)
        {
            builder.RegisterDb();
            builder.RegisterApplicationServices();
            builder.RegisterInfrastructureServices();
        }

        public static void RegisterDb(this ContainerBuilder builder)
        {
            builder.Register(c => new NpgsqlConnection(AppConfig.ConnectionStrings.DefaultConnection))
                   .As<IDbConnection>()
                   .InstancePerLifetimeScope();
        }
    }
}
