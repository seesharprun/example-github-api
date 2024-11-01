namespace Example.Console.Factories;

using System.Threading.RateLimiting;
using Example.Console.Settings;
using Microsoft.Extensions.Options;

internal sealed class TimeWindowRateLimiterFactory(
    IOptions<Configuration> configurationOptions
)
{
    private readonly Configuration configuration = configurationOptions.Value;

    public RateLimiter CreateRateLimiter() => new FixedWindowRateLimiter(
        new FixedWindowRateLimiterOptions()
        {
            PermitLimit = configuration.MaxRequests,
            Window = configuration.RequestsTimeWindow,
        }
    );
}