using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class WalletDTO
    {
        #region Properties

        public int WalletId { get; set; }
        public int UserId { get; set; } 
        public decimal Balance { get; set; }
        public DateTime? LastClaimed { get; set; }
        public bool CanClaim { get; set; }
        #endregion
    }
}