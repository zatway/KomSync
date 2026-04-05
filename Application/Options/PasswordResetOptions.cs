namespace Application.Options;

public sealed class PasswordResetOptions
{
    public const string SectionName = "PasswordReset";

    public string FrontendBaseUrl { get; set; } = "http://localhost:5173";

    public int TokenLifetimeHours { get; set; } = 24;
}
