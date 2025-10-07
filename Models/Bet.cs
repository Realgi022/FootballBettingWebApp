using System;

namespace FootballBettingWebApp.Models
{
    public class Bet
    {
        public int BetId { get; set; }
        public int UserId { get; set; }
        public int OddsId { get; set; }
        public decimal Stake { get; set; }
        public string Status { get; set; }   // Pending/Won/Lost
        public DateTime PlacedAt { get; set; }

    }

}
