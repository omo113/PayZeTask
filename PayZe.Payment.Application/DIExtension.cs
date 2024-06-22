using FluentValidation;
using PayZe.Payment.Application.BackgroundJobs;
using PayZe.Payment.Application.Behaviors;
using PayZe.Payment.Application.Services;
using PayZe.Payment.Application.Services.Interfaces;
using PayZe.Shared;
using System.Reflection;

namespace PayZe.Payment.Application;

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
        services.AddSingleton<PaymentToProcessChannel>();
        services.AddKeyedTransient<IPaymentProcessor, ServiceA>("ServiceA");
        services.AddKeyedTransient<IPaymentProcessor, ServiceB>("ServiceB");
        services.AddHostedService<PaymentProcessingService>();
        return services;
    }
}