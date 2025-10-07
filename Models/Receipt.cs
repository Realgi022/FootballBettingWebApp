using System;

namespace FootballBettingWebApp.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public int BetId { get; set; }
        public decimal AmountToWin { get; set; }
        public string Outcome { get; set; }  
    }
}
