using GetCurrentTeams.Application.Interfaces;
using GetCurrentTeams.Application.Services;
using GetCurrentTeams.Infrastructure.Http;
using GetCurrentTeams.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared;

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

    // Register our database
    services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

    // Add our services for this application
    services.AddHttpClient<ICurrentTeamsFetcher, CurrentTeamsFetcher>();
    services.AddScoped<ITeamRepository, TeamRepository>();
    services.AddScoped<TeamService>();

    // Run our program
    var provider = services.BuildServiceProvider();
    var teamService = provider.GetRequiredService<TeamService>();

    await teamService.SyncTeamsAsync();
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while syncing games");
}
finally
{

}