namespace BLL.DTOs
{
    public class MatchDto
    {
        #region Properties

        public int MatchId { get; set; }
        public string TeamHome { get; set; } = string.Empty;
        public string TeamAway { get; set; } = string.Empty;
        public DateTime Date { get; set; }   
        public string? Result { get; set; }
        public double Odds1 { get; set; }
        public double Odds2 { get; set; }
        public double OddsDraw { get; set; }
        #endregion
    }
}
