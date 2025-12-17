using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BLL.Models;
using BLL.Tests.Repositories;

namespace BLL.Tests.BetTesting
{
    [TestClass]
    public class BetRepositoryTests
    {
        private FakeBetRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeBetRepository();
        }

        [TestMethod]
        public async Task AddAsync_ShouldStoreBet()
        {
            var bet = new Bet
            {
                BetId = 1,
                UserId = 1,
                MatchId = 5,
                BetType = "1",
                Stake = 10,
                Odds = 2,
                Status = "Pending",
                PlacedAt = System.DateTime.UtcNow
            };

            await _repo.AddAsync(bet);

            var all = await _repo.GetAllAsync();
            Assert.AreEqual(1, all.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnCorrectBet()
        {
            var bet = new Bet
            {
                BetId = 99,
                UserId = 1,
                MatchId = 5,
                BetType = "1",
                Stake = 10,
                Odds = 2,
                Status = "Pending",
                PlacedAt = System.DateTime.UtcNow
            };

            await _repo.AddAsync(bet);

            var result = await _repo.GetByIdAsync(99);
            Assert.IsNotNull(result);
            Assert.AreEqual(99, result!.BetId);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyBet()
        {
            var bet = new Bet
            {
                BetId = 50,
                UserId = 1,
                MatchId = 5,
                BetType = "1",
                Stake = 10,
                Odds = 2,
                Status = "Pending",
                PlacedAt = System.DateTime.UtcNow
            };

            await _repo.AddAsync(bet);

            bet.Status = "Won";
            await _repo.UpdateAsync(bet);

            var result = await _repo.GetByIdAsync(50);
            Assert.AreEqual("Won", result!.Status);
        }
    }
}
