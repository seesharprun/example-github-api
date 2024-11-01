namespace Example.Console.Services;

using System.Net.Http.Json;
using System.Text.Json;

internal sealed class GitHubApiService(
    HttpClient client
)
{
    public async Task<T> GetDataFromGitHubAsync<T>(string endpoint)
    {
        HttpResponseMessage response = await client.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        JsonSerializerOptions options = new(){ PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

        T? result = await response.Content.ReadFromJsonAsync<T>(options)
            ?? throw new InvalidOperationException("Failed to deserialize metadata from the GitHub API.");

        return result;
    }
}