using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Interfaces;
using BLL.Models;
using BLL.DTOs;

using DomainMatch = BLL.Models.Match;

namespace BLL.Tests.BetTesting
{
    [TestClass]
    public class BetServiceTests
    {
        private Mock<IBetRepository> _betRepo = null!;
        private Mock<IMatchRepository> _matchRepo = null!;
        private Mock<IUserRepository> _userRepo = null!;
        private Mock<IWalletRepository> _walletRepo = null!;
        private BetService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _betRepo = new Mock<IBetRepository>();
            _matchRepo = new Mock<IMatchRepository>();
            _userRepo = new Mock<IUserRepository>();
            _walletRepo = new Mock<IWalletRepository>();
            _service = new BetService(_betRepo.Object, _matchRepo.Object, _userRepo.Object, _walletRepo.Object);
        }

        // -------------------- AddBetAsync --------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddBetAsync_ShouldThrow_WhenStakeInvalid()
        {
            var dto = new BetDto { Stake = 0, Odds = 1, BetType = "1", MatchId = 1 };
            await _service.AddBetAsync(dto, "john");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task AddBetAsync_ShouldThrow_WhenUserNotFound()
        {
            _userRepo.Setup(r => r.GetUserByUsernameAsync("john")).ReturnsAsync((User?)null);

            var dto = new BetDto { Stake = 10, Odds = 1.5m, BetType = "1", MatchId = 1 };
            await _service.AddBetAsync(dto, "john");
        }

        [TestMethod]
        public async Task AddBetAsync_ShouldDeductBalance_AndAddBet()
        {
            var user = new User { UserId = 1, Username = "john" };
            var wallet = new Wallet { WalletId = 1, UserId = 1, Balance = 100 };

            _userRepo.Setup(r => r.GetUserByUsernameAsync("john")).ReturnsAsync(user);
            _walletRepo.Setup(r => r.GetWalletByUserIdAsync(1)).ReturnsAsync(wallet);

            var dto = new BetDto { Stake = 20, Odds = 2m, BetType = "1", MatchId = 5 };

            await _service.AddBetAsync(dto, "john");

            Assert.AreEqual(80, wallet.Balance);
            _walletRepo.Verify(r => r.UpdateWalletAsync(wallet), Times.Once);
            _betRepo.Verify(r => r.AddAsync(It.IsAny<Bet>()), Times.Once);
        }

        // -------------------- GetAllBetsAsync --------------------

        [TestMethod]
        public async Task GetAllBetsAsync_ShouldReturnOnlyUsersBets()
        {
            var user = new User { UserId = 1, Username = "john" };

            _userRepo.Setup(r => r.GetUserByUsernameAsync("john")).ReturnsAsync(user);

            _betRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Bet>
            {
                new Bet { BetId = 1, UserId = 1, MatchId = 10, BetType = "1", Stake = 10, Odds = 2, Status="Pending", PlacedAt = DateTime.UtcNow },
                new Bet { BetId = 2, UserId = 2, MatchId = 11, BetType = "2", Stake = 20, Odds = 3, Status="Pending", PlacedAt = DateTime.UtcNow }
            });

            _matchRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DomainMatch>
            {
                new DomainMatch { MatchId = 10, TeamHome = "A", TeamAway = "B", Result = "1" },
                new DomainMatch { MatchId = 11, TeamHome = "C", TeamAway = "D", Result = "2" }
            });

            var result = (await _service.GetAllBetsAsync("john")).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].BetId);
        }

        // -------------------- ResolveBetsAsync --------------------

        [TestMethod]
        public async Task ResolveBetsAsync_ShouldMarkWinningAndLosingBets()
        {
            _betRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Bet>
            {
                new Bet { BetId = 1, MatchId = 99, BetType = "1", Status="Pending" },
                new Bet { BetId = 2, MatchId = 99, BetType = "2", Status="Pending" }
            });

            var matchDto = new MatchDto { MatchId = 99, Result = "1" };

            await _service.ResolveBetsAsync(matchDto);

            _betRepo.Verify(r => r.UpdateAsync(It.Is<Bet>(b => b.BetId == 1 && b.Status == "Won")), Times.Once);
            _betRepo.Verify(r => r.UpdateAsync(It.Is<Bet>(b => b.BetId == 2 && b.Status == "Lost")), Times.Once);
        }

        // -------------------- UpdateBetAsync --------------------

        [TestMethod]
        public async Task UpdateBetAsync_ShouldUpdateBetStatus()
        {
            var bet = new Bet { BetId = 10, Status = "Pending" };

            _betRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(bet);

            var dto = new BetDto { BetId = 10, Status = "Won" };

            await _service.UpdateBetAsync(dto);

            Assert.AreEqual("Won", bet.Status);
            _betRepo.Verify(r => r.UpdateAsync(bet), Times.Once);
        }
    }
}
