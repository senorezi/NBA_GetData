using GetTodaysGame.Application.Interfaces;
using GetTodaysGame.Core.Entities;
using Shared;

namespace GetTodaysGame.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveGamesAsync(List<Game> games)
        {
            foreach (var game in games)
            {
                _context.Games.Add(new Game
                {
                    AwayTeamName = game.AwayTeamName,
                    AwayTeamLosses = game.AwayTeamLosses,
                    AwayTeamWins = game.AwayTeamWins,
                    HomeTeamName = game.HomeTeamName,
                    HomeTeamLosses = game.HomeTeamLosses,
                    HomeTeamWins = game.HomeTeamWins,
                    DateSubmitted = DateTimeOffset.Now.ToString("yyyyMMdd")
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}
