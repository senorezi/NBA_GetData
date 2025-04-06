using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCurrentTeams.Domain.Entities
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary key property
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string DateCreated { get; set; } = string.Empty;
    }
}
