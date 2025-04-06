using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTodaysGame.Core.Entities
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AwayTeamName { get; set; } = string.Empty;
        public int AwayTeamLosses { get; set; }
        public int AwayTeamWins { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int HomeTeamLosses { get; set; }
        public int HomeTeamWins { get; set; }
        public string DateSubmitted { get; set; } = string.Empty;
    }
}
