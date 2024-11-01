namespace Example.Console.Settings;

public record Secrets
{
    public string? GitHubApiToken { get; init; } = null;
}