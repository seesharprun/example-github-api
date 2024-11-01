namespace Example.Console.Settings;

public record Configuration
{
    required public string BaseApiUrl { get; init; }

    required public int MaxRequests { get; init; }

    required public TimeSpan RequestsTimeWindow { get; init; }

    required public int RetryAttempts { get; init; }

    required public TimeSpan RetryDelayWait { get; init; }

    required public string UserAgent { get; init; }

    required public string UserAgentVersion { get; init; }
}