using GetCurrentTeams.Application.Interfaces;
using GetCurrentTeams.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace GetCurrentTeams.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task SaveTeamsAsync(List<Team> teams)
        {
            foreach (var team in teams)
            {
                _context.Teams.Add(team);
            }
            await _context.SaveChangesAsync();
        }
    }
}
