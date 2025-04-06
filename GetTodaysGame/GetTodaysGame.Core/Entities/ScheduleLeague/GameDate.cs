using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetTodaysGame.Entities.ScheduleLeague
{
    public class GameDate
    {
        [JsonPropertyName("gameDate")]
        public string Date { get; set; }

        [JsonPropertyName("games")]
        public List<LeagueScheduleGame> Games { get; set; }

    }
}
