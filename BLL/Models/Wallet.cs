using System;

namespace BLL.Models
{
    public class Wallet
    {
        private int _walletId;
        private int _userId;
        private decimal _balance;
        private DateTime? _lastClaimed;

        public int WalletId
        {
            get => _walletId;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Wallet ID cannot be negative.");
                _walletId = value;
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

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Balance cannot be negative.");
                _balance = value;
            }
        }

        public DateTime? LastClaimed
        {
            get => _lastClaimed;
            set
            {
                if (value != null && value > DateTime.UtcNow)
                    throw new ArgumentException("LastClaimed cannot be in the future.");
                _lastClaimed = value;
            }
        }
    }
}
