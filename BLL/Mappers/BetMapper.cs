using BLL.DTOs;
using BLL.Models;

namespace BLL.Mappers
{
    public static class BetMapper
    {
        public static BetDto ToDto(Bet bet, string username, Match match)
        {
            return new BetDto
            {
                BetId = bet.BetId,
                MatchId = match.MatchId,
                TeamHome = match.TeamHome,
                TeamAway = match.TeamAway,
                BetType = bet.BetType,
                Stake = bet.Stake,
                Odds = bet.Odds,
                Status = bet.Status,
                PlacedAt = bet.PlacedAt,
                Username = username
            };
        }

        public static Bet ToEntity(BetDto dto, int userId)
        {
            return new Bet
            {
                UserId = userId,
                MatchId = dto.MatchId,
                BetType = dto.BetType,
                Stake = dto.Stake,
                Odds = dto.Odds,
                Status = "Pending",
                PlacedAt = DateTime.UtcNow
            };
        }
    }
}
