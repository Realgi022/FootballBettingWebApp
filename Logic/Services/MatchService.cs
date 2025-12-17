using BLL.Interfaces;
using BLL.Models;
using BLL.DTOs;
using BLL.Mappers;     
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<MatchDto>> GetAllMatchesDtoAsync()
        {
            var matches = await _matchRepository.GetAllAsync();
            return matches.Select(x => x.ToDto());
        }

        public async Task<IEnumerable<MatchDto>> GetUpcomingMatchesDtoAsync()
        {
            var matches = await _matchRepository.GetUpcomingAsync();
            return matches.Select(x => x.ToDto());
        }

        public async Task<MatchDto?> GetMatchByIdDtoAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            return match == null ? null : MatchMapper.ToDto(match);
        }

        public async Task CreateMatchAsync(MatchDto matchDto)
        {
            var match = MatchMapper.ToModel(matchDto, isNew: true); 
            await _matchRepository.AddAsync(match);
        }

        public async Task UpdateMatchResult(MatchDto matchDto)
        {
            var match = await _matchRepository.GetByIdAsync(matchDto.MatchId);
            if (match == null) throw new InvalidOperationException("Match not found.");

            match.Result = matchDto.Result;
            await _matchRepository.UpdateAsync(match);
        }
    }
}
