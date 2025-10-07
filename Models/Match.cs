using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FootballBettingWebApp.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }

        public void UpdateResult(string result)
        {
            Result = result;
        }
    }
}
