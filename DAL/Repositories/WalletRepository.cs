using BLL.Interfaces;
using BLL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly string _connectionString;

        public WalletRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<Wallet?> GetWalletByUserIdAsync(int userId)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT WalletId, UserId, Balance, LastClaimed FROM Wallets WHERE UserId = @userId";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Wallet
                {
                    WalletId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Balance = reader.GetDecimal(2),
                    LastClaimed = reader.IsDBNull(3) ? null : reader.GetDateTime(3)
                };
            }

            return null;
        }

        public async Task CreateWalletAsync(Wallet wallet)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "INSERT INTO Wallets (UserId, Balance, LastClaimed) VALUES (@userId, @balance, @lastClaimed)";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", wallet.UserId);
            cmd.Parameters.AddWithValue("@balance", wallet.Balance);
            cmd.Parameters.AddWithValue("@lastClaimed", (object?)wallet.LastClaimed ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateWalletAsync(Wallet wallet)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "UPDATE Wallets SET Balance = @balance, LastClaimed = @lastClaimed WHERE UserId = @userId";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@balance", wallet.Balance);
            cmd.Parameters.AddWithValue("@lastClaimed", (object?)wallet.LastClaimed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@userId", wallet.UserId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Wallet?> GetWalletByIdAsync(int walletId)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT WalletId, UserId, Balance, LastClaimed FROM Wallets WHERE WalletId = @walletId";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@walletId", walletId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Wallet
                {
                    WalletId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Balance = reader.GetDecimal(2),
                    LastClaimed = reader.IsDBNull(3) ? null : reader.GetDateTime(3)
                };
            }

            return null;
        }

    }
}
