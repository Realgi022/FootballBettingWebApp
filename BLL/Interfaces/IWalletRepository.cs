using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetWalletByUserIdAsync(int userId);
        Task CreateWalletAsync(Wallet wallet);
        Task UpdateWalletAsync(Wallet wallet);
        Task<Wallet?> GetWalletByIdAsync(int walletId);

    }
}
