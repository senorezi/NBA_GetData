using GetTodaysGame.Application.Interfaces;
using GetTodaysGame.Application.Models;
using GetTodaysGame.Application.Services;
using GetTodaysGame.Core.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;

namespace GetTodaysGame.Application.Tests
{
    public class GameServiceTests
    {
        [Fact]
        public async Task SaveGameAsync_ShouldFetchDataAndSaveIt()
        {
            // Arrange
            var mockFetchData = new Mock<ITodaysGameFetcher>();
            var mockGameRepo = new Mock<IGameRepository>();
            var mockLogger = new Mock<ILogger<GameService>>();

            var games = new List<Game>
            {
                new Game {
                    AwayTeamName = "Warriors",
                    AwayTeamLosses = 12,
                    AwayTeamWins = 5,
                    HomeTeamName = "Suns",
                    HomeTeamLosses = 2,
                    HomeTeamWins = 5,
                    DateSubmitted = DateTimeOffset.Now.ToString("yyyyMMdd")
                }
            };

            mockFetchData.Setup(fd => fd.FetchTodaysGamesAsync()).ReturnsAsync(games);
            mockGameRepo.Setup(gr => gr.SaveGamesAsync(It.IsAny<List<Game>>())).Returns(Task.CompletedTask);

            var service = new GameService(mockFetchData.Object, mockGameRepo.Object, mockLogger.Object);

            // Act
            await service.SyncGamesAsnyc();

            // Assert
            mockFetchData.Verify(fd => fd.FetchTodaysGamesAsync(), Times.Once);
            mockGameRepo.Verify(gr => gr.SaveGamesAsync(It.Is<List<Game>>(g => g.Count == 1)), Times.Once);
        }
    }
}