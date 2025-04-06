using GetCurrentTeams.Domain.Entities;
using GetTodaysGame.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shared
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
