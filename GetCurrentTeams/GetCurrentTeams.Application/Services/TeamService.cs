using GetCurrentTeams.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace GetCurrentTeams.Application.Services
{
    public class TeamService
    {
        private readonly ICurrentTeamsFetcher _currentTeamsFetcher;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<TeamService> _logger;

        public TeamService(ICurrentTeamsFetcher currentTeamsFetcher, ITeamRepository teamRepository, ILogger<TeamService> logger)
        {
            _currentTeamsFetcher = currentTeamsFetcher;
            _teamRepository = teamRepository;
            _logger = logger;
        }
        public async Task SyncTeamsAsync()
        {
            // Fetch the teams
            _logger.LogInformation("STARTING SyncTeamsAsync");

            _logger.LogInformation("RUNNING FetchTeamsAsync");
            var teamsFromRequest = await _currentTeamsFetcher.FetchTeamsAsnyc();
            _logger.LogInformation($"RUNNING Found {teamsFromRequest.Count} teams from the HTTP request");

            // Get current teams
            _logger.LogInformation("RUNNING GetTeams");
            var teamsFromDb = await _teamRepository.GetTeams();
            _logger.LogInformation($"RUNNING Found {teamsFromDb.Count} teams from the database");

            // Get new teams
            var newTeams = teamsFromRequest
                .Where(team => !teamsFromDb.Any(et => et.Name == team.Name && et.City == team.City))
                .ToList();
            _logger.LogInformation($"RUNNING Found {newTeams.Count} new teams");

            // Only save the teams that were newly found
            if (newTeams.Count > 0)
            {
                // Save the newly found teams
                _logger.LogInformation($"RUNNING SaveTeamsAsync");
                await _teamRepository.SaveTeamsAsync(newTeams);
            }
        }
    }
}
