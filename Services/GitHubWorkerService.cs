using System.Text;
using System.Text.Json;
using Example.Console.Models;

namespace Example.Console.Services;

internal sealed class GitHubWorkerService(
    ILogger<GitHubWorkerService> logger,
    IHostApplicationLifetime hostApplicationLifetime,
    GitHubApiService gitHubApiService
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("GitHubService is starting.");

        stoppingToken.Register(() => logger.LogInformation("GitHubService is stopping."));

        // Dina, put your logic here
        // This example queries the public microsoftdocs/azure-docs API for open pull requests with the label azure-functions/svc
        string endpoint = "search/issues";

        string query = Uri.EscapeDataString("repo:microsoftdocs/azure-docs is:open is:pr label:azure-functions/svc");

        Payload response = await gitHubApiService.GetDataFromGitHubAsync<Payload>($"{endpoint}?q={query}");

        JsonSerializerOptions options = new (){ WriteIndented = true };
        string json = JsonSerializer.Serialize<IEnumerable<SearchResult>>(response.Items, options);

        logger.LogInformation($"Results:{Environment.NewLine}{json}");
        // End of your logic

        hostApplicationLifetime.StopApplication();
    }
}