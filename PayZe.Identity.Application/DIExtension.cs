using FluentValidation;
using PayZe.Identity.Application.Behaviors;
using System.Reflection;

namespace PayZe.Identity.Application;

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
        return services;
    }
}