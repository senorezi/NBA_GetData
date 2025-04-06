using GetTodaysGame.Application.Interfaces;
using GetTodaysGame.Application.Services;
using Shared;
using GetTodaysGame.Infrastructure.Http;
using GetTodaysGame.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// Setup configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// Use log file name from app settings json
var logFilePath = config.GetSection("logFilePath").Value;
var logFileName = config.GetSection("logFileName").Value;
var connectionString = config.GetSection("connectionString").Value;

// Setup logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: $@"{logFilePath}\{logFileName}-.txt", // The file will be named "log-20250323.txt" for example.
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

try
{
    Log.Information("Starting application");

    // Build our services
    var services = new ServiceCollection();

    // Register the IConfiguration instance
    services.AddSingleton<IConfiguration>(config);

    // Register logging to use Serilog
    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

    services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
    services.AddHttpClient<ITodaysGameFetcher, TodaysGameFetcher>();
    services.AddScoped<IGameRepository, GameRepository>();
    services.AddScoped<GameService>();

    // Run our program
    var provider = services.BuildServiceProvider();
    var gameService = provider.GetRequiredService<GameService>();

    await gameService.SyncGamesAsnyc();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while syncing games");
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}