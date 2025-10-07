namespace FootballBettingWebApp.Models
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public decimal Balance { get; set; }
        public DateTime? LastClaimed { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
