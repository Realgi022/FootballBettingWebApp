using System;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Models;
using BLL.Mappers;

namespace BLL.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
        }

        public async Task<WalletDTO> GetWalletAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) throw new Exception("User not found.");

            var wallet = await _walletRepository.GetWalletByUserIdAsync(user.UserId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = user.UserId,
                    Balance = 0,
                    LastClaimed = null
                };

                await _walletRepository.CreateWalletAsync(wallet);
            }

            return WalletMapper.ToDto(wallet);
        }

        public async Task<bool> ClaimDailyRewardAsync(string username)
        {
            const decimal dailyReward = 100;

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) throw new Exception("User not found.");

            var wallet = await _walletRepository.GetWalletByUserIdAsync(user.UserId);

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = user.UserId,
                    Balance = dailyReward,
                    LastClaimed = DateTime.UtcNow
                };

                await _walletRepository.CreateWalletAsync(wallet);
                return true;
            }

            if (wallet.LastClaimed.HasValue &&
                (DateTime.UtcNow - wallet.LastClaimed.Value).TotalHours < 24)
            {
                return false; 
            }

            wallet.Balance += dailyReward;
            wallet.LastClaimed = DateTime.UtcNow;

            await _walletRepository.UpdateWalletAsync(wallet);
            return true;
        }
        public async Task UpdateWalletAsync(WalletDTO walletDto)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletDto.WalletId);
            if (wallet == null) throw new System.Exception("Wallet not found");
            wallet.Balance = walletDto.Balance;
            await _walletRepository.UpdateWalletAsync(wallet);
        }

    }
}
