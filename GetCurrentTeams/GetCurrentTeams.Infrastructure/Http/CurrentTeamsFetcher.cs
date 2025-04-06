using GetCurrentTeams.Application.Interfaces;
using GetCurrentTeams.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Shared.AppSettings;

namespace GetCurrentTeams.Infrastructure.Http
{
    public class CurrentTeamsFetcher : ICurrentTeamsFetcher
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CurrentTeamsFetcher(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _configuration = config;
        }

        public async Task<List<Team>> FetchTeamsAsnyc()
        {
            string url = _configuration.GetSection("nbaTeamsUrl").Value;

            // for teams that have spaces... right now only Portland Trail Blazers
            List<TeamsWithSpaces> teamsWithSpaces = new List<TeamsWithSpaces>();
            _configuration.GetSection("teamsWithSpaces").Bind(teamsWithSpaces);

            string html = await _httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var teamNodes = htmlDoc.DocumentNode.SelectNodes("//a[starts-with(@href, '/nba/teams/') and not((normalize-space(.) = 'Team Draft History') or (normalize-space(.) = 'Roster') or (normalize-space(.) = 'Schedule') or (normalize-space(.) = 'Stats'))]");

            HashSet<string> teams = new HashSet<string>();

            foreach (var teamNode in teamNodes)
            {
                string teamName = teamNode.InnerText.Trim();

                if (!string.IsNullOrEmpty(teamName))
                {
                    teams.Add(teamName);
                }
            }

            List<Team> teamsDomain = new List<Team>();

            // Parse
            foreach (var team in teams)
            {
                var teamWithSpaces = teamsWithSpaces.Where(x => x.FullName == team).FirstOrDefault();

                if (teamWithSpaces != null)
                {
                    teamsDomain.Add(
                    new Team
                    {
                        Name = teamWithSpaces.TeamName,
                        City = teamWithSpaces.City,
                        DateCreated = DateTimeOffset.Now.ToString("yyyyMMdd")
                    });
                }
                else
                {
                    string[] words = team.Split(' ');

                    // Join all words except the last one.
                    string city = string.Join(" ", words.Take(words.Length - 1));
                    string name = words.Last();

                    teamsDomain.Add(
                    new Team
                    {
                        Name = name,
                        City = city,
                        DateCreated = DateTimeOffset.Now.ToString("yyyyMMdd")
                    });
                }
            }

            return teamsDomain;
        }
    }
}
