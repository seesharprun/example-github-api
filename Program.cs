using Example.Console.Factories;
using Example.Console.Handlers;
using Example.Console.Services;
using Example.Console.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<GitHubWorkerService>();
builder.Services.AddSingleton<TimeWindowRateLimiterFactory>();
builder.Services.AddTransient<GitHubApiService>();
builder.Services.AddTransient<HttpClientConfigurationService>();
builder.Services.AddTransient<PollyRetryPolicyHandler>();
builder.Services.AddTransient<RateLimitingHttpMessageHandler>();
builder.Services.Configure<Configuration>(builder.Configuration.GetSection(nameof(Configuration)));
builder.Services.Configure<Secrets>(builder.Configuration.GetSection(nameof(Secrets)));
builder.Services.AddHttpClient<GitHubApiService>(
    (serviceProvider, client) => serviceProvider.GetRequiredService<HttpClientConfigurationService>().ConfigureClient(client)
).AddHttpMessageHandler<RateLimitingHttpMessageHandler>().AddPolicyHandler(
    (serviceProvider, _) => serviceProvider.GetRequiredService<PollyRetryPolicyHandler>().GetRetryPolicy()
);

builder.Configuration.AddUserSecrets<Program>();

var host = builder.Build();

await host.RunAsync();