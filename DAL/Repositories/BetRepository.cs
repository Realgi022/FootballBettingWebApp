using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BLL.Models;
using BLL.Interfaces;
using BLL.Exceptions; 

namespace DAL.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly string _connectionString;

        public BetRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Bet>> GetAllAsync()
        {
            try
            {
                var bets = new List<Bet>();
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = "SELECT BetId, UserId, MatchId, BetType, Stake, Odds, Status, PlacedAt FROM Bets";
                await using var cmd = new SqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    bets.Add(new Bet
                    {
                        BetId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        MatchId = reader.GetInt32(2),
                        BetType = reader.GetString(3),
                        Stake = reader.GetDecimal(4),
                        Odds = reader.GetDecimal(5),
                        Status = reader.GetString(6),
                        PlacedAt = reader.GetDateTime(7)
                    });
                }

                return bets;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseOperationException("Database error while retrieving all bets.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Unexpected error while retrieving all bets.", ex);
            }
        }

        public async Task<Bet?> GetByIdAsync(int betId)
        {
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = "SELECT BetId, UserId, MatchId, BetType, Stake, Odds, Status, PlacedAt FROM Bets WHERE BetId = @id";
                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", betId);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Bet
                    {
                        BetId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        MatchId = reader.GetInt32(2),
                        BetType = reader.GetString(3),
                        Stake = reader.GetDecimal(4),
                        Odds = reader.GetDecimal(5),
                        Status = reader.GetString(6),
                        PlacedAt = reader.GetDateTime(7)
                    };
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseOperationException($"Database error while retrieving bet with ID {betId}.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Unexpected error while retrieving bet with ID {betId}.", ex);
            }
        }

        public async Task AddAsync(Bet bet)
        {
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = @"INSERT INTO Bets (UserId, MatchId, BetType, Stake, Odds, Status, PlacedAt)
                      VALUES (@userId, @matchId, @betType, @stake, @odds, @status, @placedAt);
                      SELECT CAST(SCOPE_IDENTITY() as int);";

                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", bet.UserId);
                cmd.Parameters.AddWithValue("@matchId", bet.MatchId);
                cmd.Parameters.AddWithValue("@betType", bet.BetType);
                cmd.Parameters.AddWithValue("@stake", bet.Stake);
                cmd.Parameters.AddWithValue("@odds", bet.Odds);
                cmd.Parameters.AddWithValue("@status", bet.Status);
                cmd.Parameters.AddWithValue("@placedAt", bet.PlacedAt);

                // Retrieve the generated ID
                bet.BetId = (int)await cmd.ExecuteScalarAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseOperationException("Database error while adding a new bet.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Unexpected error while adding a new bet.", ex);
            }
        }


        public async Task UpdateAsync(Bet bet)
        {
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = @"UPDATE Bets SET Status = @status WHERE BetId = @id";
                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", bet.Status);
                cmd.Parameters.AddWithValue("@id", bet.BetId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseOperationException($"Database error while updating bet with ID {bet.BetId}.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Unexpected error while updating bet with ID {bet.BetId}.", ex);
            }
        }
    }
}
