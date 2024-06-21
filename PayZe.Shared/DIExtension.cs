using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PayZe.Shared.Settings;
using StackExchange.Redis;

namespace PayZe.Shared;

public static class DIExtension
{
    public static IHostBuilder AddSettingsConfiguration(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, sc) =>
        {
            var environmentSettings = new EnvironmentSettings();
            hostContext.Configuration.GetSection(EnvironmentSettings.SectionName).Bind(environmentSettings);
            environmentSettings.ValidateAndThrow();
            sc.Configure<EnvironmentSettings>(hostContext.Configuration.GetSection(EnvironmentSettings.SectionName));
        });

        return builder;
    }
    public static IHostBuilder AddIdentityEnvVariable(this IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, sc) =>
        {
            var identitySettings = new IdentitySettings();
            hostContext.Configuration.GetSection(IdentitySettings.SectionName).Bind(identitySettings);
            identitySettings.ValidateAndThrow();
            sc.Configure<EnvironmentSettings>(hostContext.Configuration.GetSection(EnvironmentSettings.SectionName));
        });

        return builder;
    }
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var environmentSettings = sp.GetRequiredService<IOptions<EnvironmentSettings>>().Value;
            return ConnectionMultiplexer.Connect(environmentSettings.RedisConnectionString);
        });

        return services;
    }
}