using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace LeaderBoard;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddCommandLine(args);
        builder.Services
            .AddSingleton<LeaderBoardService>()
            .AddHostedService(p => p.GetRequiredService<LeaderBoardService>())
            .AddHttpClient(); // Add services to the container.

        // Razor pages
        builder.Services.AddRazorPages();

        var app = builder.Build();
        app.UseRouting();
        app.MapRazorPages();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");

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
        await TryGetData(stoppingToken);
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
        throw new Exception("This doesn't work because the Api requires a Cookie session with a login session id, and not sure how to do this.");
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