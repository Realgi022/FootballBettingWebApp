using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IBetService
    {
        Task<IEnumerable<BetDto>> GetAllBetsAsync(string username);
        Task AddBetAsync(BetDto betDto, string username);
        Task ResolveBetsAsync(MatchDto match);
        Task UpdateBetAsync(BetDto betDto);


    }
}
