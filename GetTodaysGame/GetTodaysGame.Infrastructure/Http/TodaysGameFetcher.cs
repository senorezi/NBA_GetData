using GetTodaysGame.Application.Interfaces;
using GetTodaysGame.Core.Entities;
using GetTodaysGame.Entities.ScheduleLeague;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GetTodaysGame.Infrastructure.Http
{
    public class TodaysGameFetcher : ITodaysGameFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public TodaysGameFetcher(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _configuration = config;
        }

        public async Task<List<Game>> FetchTodaysGamesAsync()
        {
            try
            {
                string scheduleLeagueUrl = _configuration.GetSection("scheduleLeaugeJson").Value;

                // read the json from url
                var responseStream = await _httpClient.GetStreamAsync(scheduleLeagueUrl);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var schedule = await JsonSerializer.DeserializeAsync<Root>(responseStream, options);

                // only get today's games
                var todaysGames = schedule.LeagueSchedule.GameDates
                    .Where(gd => DateTime.TryParse(gd.Date, out DateTime gameDate) && gameDate.Date == DateTime.Today)
                    .SelectMany(gd => gd.Games.Select(game => new
                    {
                        Date = DateTime.Parse(gd.Date),
                        game.GameId,
                        game.GameCode,
                        game.HomeTeam,
                        game.AwayTeam
                    }))
                    .OrderBy(game => game.Date)
                    .ToList();

                List<Game> todaysGamesCore = new List<Game>();

                if (todaysGames.Count() > 0)
                {
                    foreach (var game in todaysGames)
                    {

                        todaysGamesCore.Add(
                        new Game
                        {
                            AwayTeamName = game.AwayTeam.teamName,
                            AwayTeamLosses = game.AwayTeam.losses,
                            AwayTeamWins = game.AwayTeam.wins,
                            HomeTeamName = game.HomeTeam.teamName,
                            HomeTeamLosses = game.HomeTeam.losses,
                            HomeTeamWins = game.HomeTeam.wins,
                            DateSubmitted = DateTimeOffset.Now.ToString("yyyyMMdd")
                        });
                    }
                }
                return todaysGamesCore;
            } catch (Exception ex)
            {
                throw;
            }
        }
    }
}
