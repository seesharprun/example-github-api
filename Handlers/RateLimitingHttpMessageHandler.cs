namespace Example.Console.Handlers;

using System.Net;
using System.Net.Http.Headers;
using System.Threading.RateLimiting;
using Example.Console.Factories;

internal sealed class RateLimitingHttpMessageHandler(
    TimeWindowRateLimiterFactory rateLimiterFactory
) : DelegatingHandler
{
    private readonly RateLimiter rateLimiter = rateLimiterFactory.CreateRateLimiter();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using RateLimitLease lease = await rateLimiter.AcquireAsync(permitCount: 1, cancellationToken);

        if (lease.IsAcquired)
        {
            return await base.SendAsync(request, cancellationToken);
        }
        else
        {
            HttpResponseMessage response = new(HttpStatusCode.TooManyRequests);

            if (lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
            {
                response.Headers.RetryAfter = new RetryConditionHeaderValue(retryAfter);
            }

            return response;
        }
    }
}