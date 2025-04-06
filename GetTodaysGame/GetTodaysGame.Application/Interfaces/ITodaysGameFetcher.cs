using GetTodaysGame.Application.Models;
using GetTodaysGame.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTodaysGame.Application.Interfaces
{
    public interface ITodaysGameFetcher
    {
        Task<List<Game>> FetchTodaysGamesAsync();
    }
}
