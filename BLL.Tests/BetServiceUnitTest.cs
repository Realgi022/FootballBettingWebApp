using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using BLL.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLL.Tests
{
    [TestClass]
    public class BetServiceUnitTest
    {
        private Mock<IBetRepository> _betRepoMock;
        private Mock<IMatchRepository> _matchRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IWalletRepository> _walletRepoMock;
        private BetService _betService;

        [TestInitialize]
        public void Setup()
        {
            _betRepoMock = new Mock<IBetRepository>();
            _matchRepoMock = new Mock<IMatchRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _walletRepoMock = new Mock<IWalletRepository>();

            _betService = new BetService(
                _betRepoMock.Object,
                _matchRepoMock.Object,
                _userRepoMock.Object,
                _walletRepoMock.Object
            );
        }

        #region GetAllBetsAsync Tests

        [TestMethod]
        public async Task GetAllBetsAsync_ShouldReturnBets_ForValidUser()
        {
            var username = "testuser";
            var user = new User { UserId = 1, Username = username };
            var bets = new List<Bet> { new Bet { BetId = 1, MatchId = 100, UserId = user.UserId, BetType = "1", Stake = 50 } };
            var matches = new List<BLL.Models.Match> { new BLL.Models.Match { MatchId = 100, Result = "1" } };

            _userRepoMock.Setup(x => x.GetUserByUsernameAsync(username)).Returns(Task.FromResult(user));
            _betRepoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Bet>)bets));
            _matchRepoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult((IEnumerable<BLL.Models.Match>)matches));

            var result = await _betService.GetAllBetsAsync(username);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("1", result.First().BetType);
        }

        [TestMethod]
        public async Task GetAllBetsAsync_ShouldThrowInvalidBetException_ForEmptyUsername()
        {
            try
            {
                await _betService.GetAllBetsAsync("");
                Assert.Fail("Expected InvalidBetException was not thrown.");
            }
            catch (InvalidBetException ex)
            {
                StringAssert.Contains(ex.Message, "Username cannot be empty");
            }
        }

        #endregion

        #region AddBetAsync Tests

        [TestMethod]
        public async Task AddBetAsync_ShouldDeductWalletBalanceAndAddBet_WhenValid()
        {
            var username = "testuser";
            var betDto = new BetDto
            {
                Stake = 50,
                BetType = "1",
                Odds = 1.5m,
                MatchId = 100 // important
            };
            var user = new User { UserId = 1, Username = username };
            var wallet = new Wallet { WalletId = 1, UserId = user.UserId, Balance = 100 };

            _userRepoMock.Setup(x => x.GetUserByUsernameAsync(username)).Returns(Task.FromResult(user));
            _walletRepoMock.Setup(x => x.GetWalletByUserIdAsync(user.UserId)).Returns(Task.FromResult(wallet));
            _walletRepoMock.Setup(x => x.UpdateWalletAsync(It.IsAny<Wallet>())).Returns(Task.CompletedTask);
            _betRepoMock.Setup(x => x.AddAsync(It.IsAny<Bet>())).Returns(Task.CompletedTask);

            await _betService.AddBetAsync(betDto, username);

            Assert.AreEqual(50, wallet.Balance);
            _betRepoMock.Verify(x => x.AddAsync(It.IsAny<Bet>()), Times.Once);
            _walletRepoMock.Verify(x => x.UpdateWalletAsync(wallet), Times.Once);
        }


        [TestMethod]
        public async Task AddBetAsync_ShouldThrowInvalidBetException_WhenStakeIsZero()
        {
            var betDto = new BetDto { Stake = 0, BetType = "1", Odds = 1.5m };

            try
            {
                await _betService.AddBetAsync(betDto, "testuser");
                Assert.Fail("Expected InvalidBetException was not thrown.");
            }
            catch (InvalidBetException ex)
            {
                StringAssert.Contains(ex.Message, "Stake must be greater than zero");
            }
        }

        [TestMethod]
        public async Task AddBetAsync_ShouldThrowBetNotFoundException_WhenUserNotFound()
        {
            var betDto = new BetDto { Stake = 10, BetType = "1", Odds = 1.5m };
            _userRepoMock.Setup(x => x.GetUserByUsernameAsync("unknown")).Returns(Task.FromResult((User)null));

            try
            {
                await _betService.AddBetAsync(betDto, "unknown");
                Assert.Fail("Expected BetNotFoundException was not thrown.");
            }
            catch (BetNotFoundException ex)
            {
                StringAssert.Contains(ex.Message, "User not found");
            }
        }

        #endregion

        #region ResolveBetsAsync Tests

        [TestMethod]
        public async Task ResolveBetsAsync_ShouldUpdateBetStatus()
        {
            var match = new BLL.DTOs.MatchDto { MatchId = 1, Result = "1" };
            var bets = new List<Bet>
            {
                new Bet { BetId = 1, MatchId = 1, BetType = "1", Status = "Pending" },
                new Bet { BetId = 2, MatchId = 1, BetType = "2", Status = "Pending" }
            };

            _betRepoMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Bet>)bets));
            _betRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Bet>())).Returns(Task.CompletedTask);

            await _betService.ResolveBetsAsync(match);

            Assert.AreEqual("Won", bets.First(b => b.BetId == 1).Status);
            Assert.AreEqual("Lost", bets.First(b => b.BetId == 2).Status);
        }

        #endregion

        #region UpdateBetAsync Tests

        [TestMethod]
        public async Task UpdateBetAsync_ShouldUpdateStatus_WhenBetExists()
        {
            var betDto = new BetDto { BetId = 1, Status = "Won" };
            var bet = new Bet { BetId = 1, Status = "Pending" };

            _betRepoMock.Setup(x => x.GetByIdAsync(betDto.BetId)).Returns(Task.FromResult(bet));
            _betRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Bet>())).Returns(Task.CompletedTask);

            await _betService.UpdateBetAsync(betDto);

            Assert.AreEqual("Won", bet.Status);
        }

        [TestMethod]
        public async Task UpdateBetAsync_ShouldThrowBetNotFoundException_WhenBetDoesNotExist()
        {
            var betDto = new BetDto { BetId = 99, Status = "Won" };
            _betRepoMock.Setup(x => x.GetByIdAsync(betDto.BetId)).Returns(Task.FromResult((Bet)null));

            try
            {
                await _betService.UpdateBetAsync(betDto);
                Assert.Fail("Expected BetNotFoundException was not thrown.");
            }
            catch (BetNotFoundException ex)
            {
                StringAssert.Contains(ex.Message, "Bet not found");
            }
        }

        #endregion
    }
}
