using BLL.DTOs;
using BLL.Models;

namespace BLL.Mappers
{
    public static class MatchMapper
    {
        public static MatchDto ToDto(this Match match)
        {
            return new MatchDto
            {
                MatchId = match.MatchId,
                TeamHome = match.TeamHome,
                TeamAway = match.TeamAway,
                Date = match.Date,
                Result = match.Result,
                Odds1 = match.Odds1,
                Odds2 = match.Odds2,
                OddsDraw = match.OddsDraw
            };
        }

        public static Match ToModel(this MatchDto dto, bool isNew = false)
        {
            var match = new Match
            {
                TeamHome = dto.TeamHome,
                TeamAway = dto.TeamAway,
                Date = dto.Date,
                Result = dto.Result,
                Odds1 = dto.Odds1,
                Odds2 = dto.Odds2,
                OddsDraw = dto.OddsDraw
            };

            if (!isNew)
                match.MatchId = dto.MatchId; 

            return match;
        }

    }
}
