using System;

namespace FootballBettingWebApp.Models
{
    public enum BetType { WIN, DRAW, LOSE }

    public class Odd
    {
        public int OddsId { get; set; }
        public int MatchId { get; set; }     // Fk in Db
        public BetType BetType { get; set; }
        public decimal Value { get; set; }   
    }
}
