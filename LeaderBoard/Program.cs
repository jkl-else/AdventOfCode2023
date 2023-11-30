using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LeaderBoard;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config => config.AddCommandLine(args))
            .ConfigureServices(service =>
                    service
                        .AddSingleton<LeaderBoardService>()
                        .AddHostedService(p => p.GetRequiredService<LeaderBoardService>())
                        .AddHttpClient() // Add services to the container.
            );

        var app = builder.Build();

        //app.MapGet("leaderboard", (LeaderBoardService service) =>
        //{
        //    return service.LeaderBoardModel;
        //});

        await app.RunAsync();
    }
}

internal class LeaderBoardService : BackgroundService
{
    /// <summary>
    /// application configuration
    /// </summary>
    private IConfiguration Configuration { get; }
    /// <summary>
    /// factory to create web clients
    /// </summary>
    private IHttpClientFactory HttpClientFactory { get; }
    /// <summary>
    /// Updated model
    /// </summary>
    public LeaderBoardModel? LeaderBoardModel { get; private set; }
    /// <summary>
    /// Url to query
    /// </summary>
    private string ApiUrl { get; }
    /// <summary>
    /// Number of calls in a row that has failed
    /// </summary>
    public int FailedCalls { get; private set;}
    /// <summary>
    /// Time when last successfull call was made to Api
    /// </summary>
    public DateTime LastCall { get; private set;}

    public LeaderBoardService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        Configuration = configuration;
        HttpClientFactory = httpClientFactory;
        ApiUrl = Configuration.GetValue<string>(nameof(ApiUrl))
                 ?? throw new NullReferenceException($"Can't find {nameof(ApiUrl)} in configuration");
    }
    /// <summary>
    /// Application execution
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"Starting application to call Api: {ApiUrl}");
        // create callback timer
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(Configuration.GetValue<int>("RequestInterval")));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            // call api to get data
            if (!await TryGetData(stoppingToken))
            {
                FailedCalls++;
                Console.WriteLine($"Failed to call {FailedCalls} times.");
                continue;
            }

            if (FailedCalls > 0)
                FailedCalls = 0; // reset failed calls
            // update time for last successfull call
            LastCall = DateTime.Now;
        }
    }
    /// <summary>
    /// Get data from Api
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    internal async Task<bool> TryGetData(CancellationToken cancellationToken)
    {
        // create a client
        var client = HttpClientFactory.CreateClient();
        // get value from api
        var dataString = await client.GetStringAsync(ApiUrl, cancellationToken);
        if (string.IsNullOrEmpty(dataString))
        {
            Console.WriteLine("Failed to retrieve data from url.");
            return false;
        }
        // Deserialize retrieved data
        LeaderBoardModel = JsonSerializer.Deserialize<LeaderBoardModel>(dataString);
        return true;
    }
}
/// <summary>
/// Model for api
/// </summary>
internal class LeaderBoardModel
{
    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }
    [JsonPropertyName("event")]
    public int Event { get; set; }

    internal class MemberModel
    {
        [JsonPropertyName("global_score")]
        public int GlobalScore { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("completion_day_level")]
        public Dictionary<int, Dictionary<int, SolutionModel>> CompletionDayLevel { get; set; } = null!;
        [JsonPropertyName("local_score")]
        public int LocalScore { get; set; }
        [JsonPropertyName("stars")]
        public long Stars { get; set; }
        [JsonPropertyName("last_star_ts")]
        public long LastStarTs { get; set; }

        internal class SolutionModel
        {
            [JsonPropertyName("get_star_ts")]
            public long GetStarTs { get; set; }
            [JsonPropertyName("star_index")]
            public long StarIndex { get; set; }
        }
    }
}