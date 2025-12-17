namespace BLL.Models
{
    public class Match
    {
        private int _matchId;
        private string _teamHome = string.Empty;
        private string _teamAway = string.Empty;
        private DateTime _date;
        private string? _result;

        private double _odds1;
        private double _odds2;
        private double _oddsDraw;

        public int MatchId
        {
            get => _matchId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Match ID must be positive.");
                _matchId = value;
            }
        }

        public string TeamHome
        {
            get => _teamHome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Home team name cannot be empty.");
                _teamHome = value.Trim();
            }
        }

        public string TeamAway
        {
            get => _teamAway;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Away team name cannot be empty.");
                _teamAway = value.Trim();
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (value.Year < 2000)
                    throw new ArgumentException("Match date is not valid.");
                _date = value;
            }
        }

        public string? Result
        {
            get => _result;
            set
            {
                if (value != null)
                {
                    var validResults = new[] { "1", "X", "2" };
                    if (!validResults.Contains(value))
                        throw new ArgumentException("Result must be '1', 'X', or '2'.");
                }
                _result = value;
            }
        }

        public double Odds1
        {
            get => _odds1;
            set
            {
                if (value < 1.01)
                    throw new ArgumentException("Odds must be at least 1.01.");
                _odds1 = value;
            }
        }

        public double Odds2
        {
            get => _odds2;
            set
            {
                if (value < 1.01)
                    throw new ArgumentException("Odds must be at least 1.01.");
                _odds2 = value;
            }
        }

        public double OddsDraw
        {
            get => _oddsDraw;
            set
            {
                if (value < 1.01)
                    throw new ArgumentException("Odds must be at least 1.01.");
                _oddsDraw = value;
            }
        }
    }
}
