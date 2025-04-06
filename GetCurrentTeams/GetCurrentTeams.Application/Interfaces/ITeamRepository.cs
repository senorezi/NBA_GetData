using GetCurrentTeams.Domain.Entities;

namespace GetCurrentTeams.Application.Interfaces
{
    public interface ITeamRepository
    {
        Task SaveTeamsAsync(List<Team> teams);
        Task<List<Team>> GetTeams();
    }
}
