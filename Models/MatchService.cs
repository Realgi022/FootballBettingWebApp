namespace FootballBettingWebApp.Models
{
    public class MatchService
    {
        public List<Match> MatchCollection { get; set; } = new List<Match>();
        public void AddMatch(Match match) { }
        public void EditMatch(Match match) { /* TODO */ }
        public void SetOdds(Match match, Odd odds) { /* TODO */ }
        public void SettleMatch(Match match, string result) => match.UpdateResult(result);
    }
}
