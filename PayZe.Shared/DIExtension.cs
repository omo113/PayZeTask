using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayZe.Shared.Settings;

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
}