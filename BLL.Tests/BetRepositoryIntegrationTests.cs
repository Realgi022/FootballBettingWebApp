using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BLL.Tests.BetTesting
{
    [TestClass]
    public class BetRepositoryIntegrationTests
    {
        private BetRepository _repo = null!;
        private string _connectionString = "Server=CLLEVIOSERBIANO;Database=BettingApp_Test;Trusted_Connection=True;TrustServerCertificate=True;";

        [TestInitialize]
        public async Task Setup()
        {
            _repo = new BetRepository(_connectionString);

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "DELETE FROM Bets; DBCC CHECKIDENT('Bets', RESEED, 0);";
            await using var cmd = new SqlCommand(sql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        #region Happy Path Tests

        [TestMethod]
        public async Task AddAsync_ShouldInsertBetIntoDatabase()
        {
            try
            {
                var bet = new Bet
                {
                    UserId = 1,
                    MatchId = 1,
                    BetType = "1",
                    Stake = 10,
                    Odds = 2,
                    Status = "Pending",
                    PlacedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(bet);
                var result = await _repo.GetByIdAsync(bet.BetId);

                Assert.IsNotNull(result);
                Assert.AreEqual("1", result!.BetType);
                Assert.AreEqual(1, result.UserId);
                Assert.AreEqual(1, result.MatchId);
                Assert.AreEqual(10m, result.Stake);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyBetStatus()
        {
            try
            {
                var bet = new Bet
                {
                    UserId = 2,
                    MatchId = 2,
                    BetType = "X",
                    Stake = 5,
                    Odds = 1.5m,
                    Status = "Pending",
                    PlacedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(bet);

                bet.Status = "Won";
                await _repo.UpdateAsync(bet);

                var updatedBet = await _repo.GetByIdAsync(bet.BetId);
                Assert.IsNotNull(updatedBet);
                Assert.AreEqual("Won", updatedBet!.Status);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnInsertedBets()
        {
            try
            {
                var bet1 = new Bet { UserId = 1, MatchId = 1, BetType = "1", Stake = 10, Odds = 2, Status = "Pending", PlacedAt = DateTime.UtcNow };
                var bet2 = new Bet { UserId = 2, MatchId = 2, BetType = "X", Stake = 5, Odds = 1.5m, Status = "Pending", PlacedAt = DateTime.UtcNow };

                await _repo.AddAsync(bet1);
                await _repo.AddAsync(bet2);

                var allBets = (await _repo.GetAllAsync()).ToList();

                var insertedBets = allBets.Where(b => b.BetId == bet1.BetId || b.BetId == bet2.BetId).ToList();

                Assert.AreEqual(2, insertedBets.Count, "GetAllAsync did not return the correct number of inserted bets.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.Message);
            }
        }

        #endregion

        #region Edge Case Tests

        [TestMethod]
        public async Task AddAsync_ShouldThrow_WhenMatchIdIsZero()
        {
            try
            {
                var bet = new Bet
                {
                    UserId = 1,
                    MatchId = 0, 
                    BetType = "1",
                    Stake = 10,
                    Odds = 2,
                    Status = "Pending",
                    PlacedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(bet);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Match ID must be positive");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception type: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task AddAsync_ShouldThrow_WhenUserIdIsZero()
        {
            try
            {
                var bet = new Bet
                {
                    UserId = 0,
                    MatchId = 1,
                    BetType = "1",
                    Stake = 10,
                    Odds = 2,
                    Status = "Pending",
                    PlacedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(bet);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "User ID must be positive");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception type: " + ex.Message);
            }
        }

        [TestMethod]
        public async Task AddAsync_ShouldThrow_WhenBetTypeInvalid()
        {
            try
            {
                var bet = new Bet
                {
                    UserId = 1,
                    MatchId = 1,
                    BetType = "invalid",
                    Stake = 10,
                    Odds = 2,
                    Status = "Pending",
                    PlacedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(bet);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "BetType must be '1', 'X', or '2'");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception type: " + ex.Message);
            }
        }

        #endregion
    }
}
