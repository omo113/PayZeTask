﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PayZe.Identity.Infrastructure.Persistence;
using PayZe.Shared.Settings;

namespace PayZe.Identity.RabbitMq;

public static class DIExtension
{
    public static IHostBuilder ConfigureMessageBus(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, sc) =>
        {
            sc.AddMassTransit((x) =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("identity", false));

                // Configure outbox for message retry
                x.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseMessageRetry(r => r.Intervals(100, 500, 1000, 5000, 10000));
                    cfg.UseEntityFrameworkOutbox<IdentityDbContext>(context);
                });

                // Configure the Entity Framework outbox
                x.AddEntityFrameworkOutbox<IdentityDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(1);
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                // Configure RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                    {
                        r.Handle<RabbitMqConnectionException>();
                        r.Interval(3, TimeSpan.FromSeconds(10));
                    });
                    cfg.PrefetchCount = 10;
                    cfg.Durable = true;
                    cfg.ExchangeType = "fanout";
                    var environmentSettings = context.GetRequiredService<IOptions<EnvironmentSettings>>().Value;
                    cfg.Host(environmentSettings.RabbitMqHost, "/", host =>
                    {
                        host.Username(environmentSettings.RabbitMqUser);
                        host.Password(environmentSettings.RabbitMqPassword);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        });

        return builder;
    }
}