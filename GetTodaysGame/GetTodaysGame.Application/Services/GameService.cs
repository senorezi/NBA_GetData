using GetTodaysGame.Application.Interfaces;
using Microsoft.Extensions.Logging;


namespace GetTodaysGame.Application.Services
{
    public class GameService
    {
        private readonly ITodaysGameFetcher _todaysGameFetcher;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GameService> _logger;
        public GameService(ITodaysGameFetcher todaysGameFetcher, IGameRepository gameRepository, ILogger<GameService> logger)
        {
           _todaysGameFetcher = todaysGameFetcher;
           _gameRepository = gameRepository;
           _logger = logger;
        }
        public async Task SyncGamesAsnyc()
        {
            _logger.LogInformation("SyncGamesAsnyc started");

            var data = await _todaysGameFetcher.FetchTodaysGamesAsync();
            _logger.LogInformation("Fetched {Count} games", data.Count);

            if (data.Count > 0)
            {
                await _gameRepository.SaveGamesAsync(data);
                _logger.LogInformation("Saved {Count} games", data.Count);
            }
            else
            {
                _logger.LogInformation("No games found");
            }        
        }
    }
}
