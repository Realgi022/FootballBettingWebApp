using System;

namespace BLL.DTOs
{
    public class BetDto
    {
        #region Properties
        public int BetId { get; set; }
        public int MatchId { get; set; }
        public string TeamHome { get; set; } = string.Empty;
        public string TeamAway { get; set; } = string.Empty;
        public string BetType { get; set; } = string.Empty;
        public decimal Stake { get; set; }
        public decimal Odds { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime PlacedAt { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal AmountToWin => Stake * Odds;

        #endregion
    }
}
