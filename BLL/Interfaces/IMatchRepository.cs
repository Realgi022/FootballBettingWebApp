using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllAsync();
        Task<IEnumerable<Match>> GetUpcomingAsync();
        Task<Match?> GetByIdAsync(int matchId);
        Task AddAsync(Match match);
        Task UpdateAsync(Match match);
    }
}
