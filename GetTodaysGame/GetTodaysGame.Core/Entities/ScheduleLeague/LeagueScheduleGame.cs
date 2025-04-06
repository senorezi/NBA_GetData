using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetTodaysGame.Entities.ScheduleLeague
{
    public class LeagueScheduleGame
    {
        public string GameId { get; set; }
        public string GameCode { get; set; }
        public Team HomeTeam { get; set; }

        public Team AwayTeam { get; set; }
    }
}
