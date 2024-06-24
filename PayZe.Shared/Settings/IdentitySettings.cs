using FluentValidation;

namespace PayZe.Shared.Settings;

public class IdentitySettings
{
    private readonly IdentitySettingsValidator _validator = new();
    public const string SectionName = "settings";
    public string IdentityUrl { get; set; }
    public string Port { get; set; }
    public void ValidateAndThrow()
    {
        _validator.ValidateAndThrow(this);
    }
}

public class IdentitySettingsValidator : AbstractValidator<IdentitySettings>
{
    public IdentitySettingsValidator()
    {
        RuleFor(x => x.IdentityUrl)
            .NotNull()
            .NotEmpty();
    }
}