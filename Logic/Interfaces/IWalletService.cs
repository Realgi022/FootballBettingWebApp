using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDTO> GetWalletAsync(string username);
        Task<bool> ClaimDailyRewardAsync(string username);
        Task UpdateWalletAsync(WalletDTO wallet);

    }
}
