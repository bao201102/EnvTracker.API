using Autofac;
using Autofac.Extensions.DependencyInjection;
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
            // Thêm các dịch vụ từ IServiceCollection vào Autofac ContainerBuilder
            var services = new ServiceCollection();

            // Đăng ký dịch vụ trong IServiceCollection
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            services.ConfigureOptions<JwtBearerOptionsSetup>();

            // Chuyển dịch vụ sang Autofac
            builder.Populate(services);

            // Đăng ký dịch vụ trực tiếp với Autofac
            builder.RegisterType<JwtTokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<DapperRepository>().As<IRepository>().InstancePerLifetimeScope();
        }
    }
}
