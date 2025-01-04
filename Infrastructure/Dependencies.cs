using Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region SqlServer

            var dbLogin = Environment.GetEnvironmentVariable("DEVELOPER_LOGIN") ?? string.Empty;
            var dbPassword = Environment.GetEnvironmentVariable("DEVELOPER_PWD") ?? string.Empty;
            var dbName = Environment.GetEnvironmentVariable("DEVELOPER_DB") ?? string.Empty;

            var connectionString = string.Format(configuration.GetConnectionString("sqlserver") ?? string.Empty, dbLogin, dbPassword, dbName);

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            #endregion

            #region MassTransit

            services.AddMassTransit(x =>
            {

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    // "/" - rabbitmq virtual host -> "/" by default (rabbitmq.conf)
                    cfg.Host("localhost", "/", cred =>
                    {
                        cred.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? string.Empty);
                        cred.Password(Environment.GetEnvironmentVariable("RABBITMQ_USERPASS") ?? string.Empty);
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            #endregion
            return services;
        }
    }
}
