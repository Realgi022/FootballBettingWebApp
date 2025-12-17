using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using System;

namespace BLL.Tests.BetTesting
{
    [TestClass]
    public class BetTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BetId_ShouldThrow_WhenNegative()
        {
            var bet = new Bet();
            bet.BetId = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserId_ShouldThrow_WhenZeroOrNegative()
        {
            var bet = new Bet();
            bet.UserId = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MatchId_ShouldThrow_WhenZeroOrNegative()
        {
            var bet = new Bet();
            bet.MatchId = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BetType_ShouldThrow_WhenInvalid()
        {
            var bet = new Bet();
            bet.BetType = "ABC";
        }

        [TestMethod]
        public void BetType_ShouldAccept_ValidValues()
        {
            var bet = new Bet();

            bet.BetType = "1";
            Assert.AreEqual("1", bet.BetType);

            bet.BetType = "X";
            Assert.AreEqual("X", bet.BetType);

            bet.BetType = "2";
            Assert.AreEqual("2", bet.BetType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stake_ShouldThrow_WhenZeroOrNegative()
        {
            var bet = new Bet();
            bet.Stake = 0;
        }

        [TestMethod]
        public void Stake_ShouldAccept_PositiveValue()
        {
            var bet = new Bet();
            bet.Stake = 10;
            Assert.AreEqual(10, bet.Stake);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Odds_ShouldThrow_WhenLessThanOne()
        {
            var bet = new Bet();
            bet.Odds = 0.5m;
        }

        [TestMethod]
        public void Odds_ShouldAccept_OneOrHigher()
        {
            var bet = new Bet();
            bet.Odds = 1.5m;
            Assert.AreEqual(1.5m, bet.Odds);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Status_ShouldThrow_WhenEmpty()
        {
            var bet = new Bet();
            bet.Status = " ";
        }

        [TestMethod]
        public void Status_ShouldAccept_ValidValue()
        {
            var bet = new Bet();
            bet.Status = "Won";
            Assert.AreEqual("Won", bet.Status);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlacedAt_ShouldThrow_WhenInFuture()
        {
            var bet = new Bet();
            bet.PlacedAt = DateTime.UtcNow.AddHours(5);
        }

        [TestMethod]
        public void PlacedAt_ShouldAccept_Today()
        {
            var bet = new Bet();
            var now = DateTime.UtcNow;
            bet.PlacedAt = now;
            Assert.AreEqual(now, bet.PlacedAt);
        }
    }
}
