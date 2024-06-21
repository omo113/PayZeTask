using FluentValidation;
using PayZe.Orders.Application.Behaviors;
using PayZe.Orders.Application.Services;
using PayZe.Orders.Application.Services.Interfaces;
using PayZe.Shared;
using System.Reflection;

namespace PayZe.Orders.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), filter: result =>
        {
            foreach (var constructor in result.ValidatorType.GetConstructors())
            {
                return !constructor.GetParameters()
                    .Any(x => x.ParameterType.IsPrimitive);
            }

            return true;
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IIdentityCompanyGrpcClient, IdentityCompanyGrpcClient>();
        services.AddSingleton<CurrencyDictionary>();
        return services;
    }
}