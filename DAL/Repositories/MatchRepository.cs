using BLL.Interfaces;
using BLL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly string _connectionString;

        public MatchRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            var matches = new List<Match>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"SELECT MatchId, TeamHome, TeamAway, MatchDate, Result, Odds1, Odds2, OddsDraw 
                          FROM Matches ORDER BY MatchDate";
            await using var cmd = new SqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                matches.Add(new Match
                {
                    MatchId = reader.GetInt32(0),
                    TeamHome = reader.GetString(1),
                    TeamAway = reader.GetString(2),
                    Date = reader.GetDateTime(3),
                    Result = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Odds1 = reader.GetDouble(5),
                    Odds2 = reader.GetDouble(6),
                    OddsDraw = reader.GetDouble(7)
                });
            }

            return matches;
        }

        public async Task<IEnumerable<Match>> GetUpcomingAsync()
        {
            var matches = new List<Match>();

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"SELECT MatchId, TeamHome, TeamAway, MatchDate, Result, Odds1, Odds2, OddsDraw
                          FROM Matches 
                          WHERE MatchDate >= @now 
                          ORDER BY MatchDate";

            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@now", DateTime.Now);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                matches.Add(new Match
                {
                    MatchId = reader.GetInt32(0),
                    TeamHome = reader.GetString(1),
                    TeamAway = reader.GetString(2),
                    Date = reader.GetDateTime(3),
                    Result = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Odds1 = reader.GetDouble(5),
                    Odds2 = reader.GetDouble(6),
                    OddsDraw = reader.GetDouble(7)
                });
            }

            return matches;
        }

        public async Task<Match?> GetByIdAsync(int matchId)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"SELECT MatchId, TeamHome, TeamAway, MatchDate, Result, Odds1, Odds2, OddsDraw 
                          FROM Matches WHERE MatchId = @id";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", matchId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Match
                {
                    MatchId = reader.GetInt32(0),
                    TeamHome = reader.GetString(1),
                    TeamAway = reader.GetString(2),
                    Date = reader.GetDateTime(3),
                    Result = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Odds1 = reader.GetDouble(5),
                    Odds2 = reader.GetDouble(6),
                    OddsDraw = reader.GetDouble(7)
                };
            }

            return null;
        }

        public async Task AddAsync(Match match)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"INSERT INTO Matches 
                          (TeamHome, TeamAway, MatchDate, Result, Odds1, Odds2, OddsDraw)
                          VALUES (@home, @away, @date, @result, @odds1, @odds2, @oddsDraw)";

            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@home", match.TeamHome);
            cmd.Parameters.AddWithValue("@away", match.TeamAway);
            cmd.Parameters.AddWithValue("@date", match.Date);
            cmd.Parameters.AddWithValue("@result", (object?)match.Result ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@odds1", match.Odds1);
            cmd.Parameters.AddWithValue("@odds2", match.Odds2);
            cmd.Parameters.AddWithValue("@oddsDraw", match.OddsDraw);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Match match)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "UPDATE Matches SET Result = @result WHERE MatchId = @id";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@result", (object?)match.Result ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", match.MatchId);

            await cmd.ExecuteNonQueryAsync();
        }

    }
}
