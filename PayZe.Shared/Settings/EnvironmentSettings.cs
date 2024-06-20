using FluentValidation;

namespace PayZe.Shared.Settings;

public class EnvironmentSettings
{
    private readonly EnvironmentSettingsValidator _validator = new();
    public const string SectionName = "settings";
    public string DatabaseConnection { get; set; }
    public string RabbitMqHost { get; set; }
    public string RabbitMqPassword { get; set; }
    public string RabbitMqUser { get; set; }

    public void ValidateAndThrow()
    {
        _validator.ValidateAndThrow(this);
    }
}
public class EnvironmentSettingsValidator : AbstractValidator<EnvironmentSettings>
{
    public EnvironmentSettingsValidator()
    {
        RuleFor(x => x.DatabaseConnection)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.RabbitMqHost)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.RabbitMqPassword)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.RabbitMqUser)
            .NotNull()
            .NotEmpty();

    }
}