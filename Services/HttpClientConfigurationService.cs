namespace Example.Console.Services;

using System.Net.Http.Headers;
using Example.Console.Settings;
using Microsoft.Extensions.Options;

public sealed class HttpClientConfigurationService(
    IOptions<Configuration> configurationOptions,
    IOptions<Secrets> secretsOptions
)
{
    private readonly Configuration configuration = configurationOptions.Value;
    private readonly Secrets secrets = secretsOptions.Value;

    public void ConfigureClient(HttpClient client)
    {
        client.BaseAddress = new Uri(configuration.BaseApiUrl);

        client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        client.DefaultRequestHeaders.UserAgent.Add(
            item: new ProductInfoHeaderValue(
                productName: configuration.UserAgent,
                productVersion: configuration.UserAgentVersion
            )
        );

        if (secrets.GitHubApiToken is not null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                scheme: "Bearer",
                parameter: secrets.GitHubApiToken
            );
        }
    }
}