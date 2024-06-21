using Microsoft.OpenApi.Models;

namespace PayZe.Orders.Api;

public static class DIExtension
{
    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("API-Key", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "API-Key",
                Type = SecuritySchemeType.ApiKey,
                Description = "API Key required for authentication"
            });

            c.AddSecurityDefinition("API-Secret", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "API-Secret",
                Type = SecuritySchemeType.ApiKey,
                Description = "API Secret required for authentication"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "API-Key"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "API-Secret"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}