namespace FootballBettingWebApp.Models
{
    public class Gambler : User
    {
        public decimal Balance { get; set; }       
        public Wallet Wallet { get; set; }
    }
}
