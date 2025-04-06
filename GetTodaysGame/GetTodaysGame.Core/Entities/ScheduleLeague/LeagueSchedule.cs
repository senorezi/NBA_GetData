using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTodaysGame.Entities.ScheduleLeague
{
    public class LeagueSchedule
    {
        public string SeasonYear { get; set; }
        public string LeagueId { get; set; }
        public List<GameDate> GameDates { get; set; }
    }
}
