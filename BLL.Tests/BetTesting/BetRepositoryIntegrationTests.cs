using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Interfaces;
using BLL.Models;
using DAL.Repositories; 
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Tests.BetTesting
{
    [TestClass]
    public class BetRepositoryIntegrationTests
    {
        private BetRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            var connectionString = "Server=CLLEVIOSERBIANO;Database=BettingApp_Test;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new BetRepository(connectionString);
        }

        [TestMethod]
        public async Task AddAsync_ShouldInsertBetIntoDatabase()
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

            var bets = await _repo.GetAllAsync();
            Assert.IsTrue(bets.Any(b => b.UserId == 1 && b.MatchId == 1 && b.Stake == 10));
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnInsertedBet()
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

            var allBets = (await _repo.GetAllAsync()).ToList();
            var insertedBet = allBets.Last();

            var result = await _repo.GetByIdAsync(insertedBet.BetId);
            Assert.IsNotNull(result);
            Assert.AreEqual("X", result!.BetType);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyBetStatus()
        {
            var bet = new Bet
            {
                UserId = 3,
                MatchId = 3,
                BetType = "2",
                Stake = 20,
                Odds = 3,
                Status = "Pending",
                PlacedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(bet);

            var allBets = (await _repo.GetAllAsync()).ToList();
            var insertedBet = allBets.Last();
            insertedBet.Status = "Won";

            await _repo.UpdateAsync(insertedBet);

            var updatedBet = await _repo.GetByIdAsync(insertedBet.BetId);
            Assert.AreEqual("Won", updatedBet!.Status);
        }
    }
}
