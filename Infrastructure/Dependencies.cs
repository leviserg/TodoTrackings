using Infrastructure.Messaging;
using Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    // "/" - rabbitmq virtual host -> "/" by default (rabbitmq.conf)
                    cfg.Host("localhost", "/", cred =>
                    {
                        cred.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? string.Empty);
                        cred.Password(Environment.GetEnvironmentVariable("RABBITMQ_USERPASS") ?? string.Empty);
                    });

                    cfg.Message<TodoTaskChangedMessage>(configTopology =>
                    {
                        configTopology.SetEntityName("my-queue");
                    });

                    cfg.ConfigureEndpoints(ctx);

                    cfg.UseDelayedMessageScheduler();
                    cfg.UseMessageRetry(r => r.Immediate(5)); // Retry 5 times before faulting

                });
            });

            #endregion

            

            return services;
        }
    }
}
