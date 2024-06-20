using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayZe.Identity.Infrastructure.Persistence;

namespace PayZe.Identity.RabbitMq;

public static class DIExtension
{
    public static IServiceCollection ConfigureMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddEntityFrameworkOutbox<CompanyIdentityDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(10);
                o.UsePostgres();
                o.UseBusOutbox();
            });
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseMessageRetry(r =>
                {
                    r.Handle<RabbitMqConnectionException>();
                    r.Interval(3, TimeSpan.FromSeconds(10));
                });

                cfg.Host(configuration["RabbitMq:Host"], "/", host =>
                {
                    host.Username(configuration.GetSection("RabbitMq:Username").Value!);
                    host.Password(configuration.GetSection("RabbitMq:Password").Value!);
                });
                cfg.ConfigureEndpoints(context);

            });

        });
        return services;
    }
}