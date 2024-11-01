namespace Example.Console.Handlers;

using Example.Console.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

public class PollyRetryPolicyHandler(
    IOptions<Configuration> configurationOptions
)
{
    private readonly Configuration configuration = configurationOptions.Value;

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(
                retryCount: configuration.RetryAttempts,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(configuration.RetryDelayWait.Seconds, retryAttempt))
            );
}