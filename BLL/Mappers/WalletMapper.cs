using BLL.DTOs;
using BLL.Models;

namespace BLL.Mappers
{
    public static class WalletMapper
    {
        public static WalletDTO ToDto(Wallet wallet)
        {
            return new WalletDTO
            {
                WalletId = wallet.WalletId,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                LastClaimed = wallet.LastClaimed,
                CanClaim = !wallet.LastClaimed.HasValue ||
                           (DateTime.UtcNow - wallet.LastClaimed.Value).TotalHours >= 24
            };
        }

        public static Wallet ToModel(WalletDTO dto)
        {
            return new Wallet
            {
                WalletId = dto.WalletId,
                UserId = dto.UserId,
                Balance = dto.Balance,
                LastClaimed = dto.LastClaimed
            };
        }
    }
}
