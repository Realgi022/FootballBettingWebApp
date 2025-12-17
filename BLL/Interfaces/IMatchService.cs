using BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllMatchesDtoAsync();
        Task<IEnumerable<MatchDto>> GetUpcomingMatchesDtoAsync();
        Task<MatchDto?> GetMatchByIdDtoAsync(int matchId);
        Task CreateMatchAsync(MatchDto matchDto);
        Task UpdateMatchResult(MatchDto matchDto);

    }
}
