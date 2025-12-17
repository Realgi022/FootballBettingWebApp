using System;

namespace BLL.Models
{

    public class Bet
    {
        private int _betId;
        private int _userId;  
        private int _matchId;
        private string _betType = string.Empty;
        private decimal _stake;
        private decimal _odds;
        private string _status = "Pending";
        private DateTime _placedAt;

        public int BetId
        {
            get => _betId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Bet ID cannot be negative.");
                _betId = value;
            }
        }

        public int UserId  
        {
            get => _userId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("User ID must be positive.");
                _userId = value;
            }
        }

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

        public string BetType
        {
            get => _betType;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("BetType cannot be empty.");
                if (value != "1" && value != "X" && value != "2")
                    throw new ArgumentException("BetType must be '1', 'X', or '2'.");
                _betType = value;
            }
        }

        public decimal Stake
        {
            get => _stake;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Stake must be greater than 0.");
                _stake = value;
            }
        }

        public decimal Odds
        {
            get => _odds;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Odds must be 1 or higher.");
                _odds = value;
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be empty.");
                _status = value;
            }
        }

        public DateTime PlacedAt
        {
            get => _placedAt;
            set
            {
                if (value > DateTime.UtcNow)
                    throw new ArgumentException("PlacedAt cannot be in the future.");
                _placedAt = value;
            }
        }
    }
}
