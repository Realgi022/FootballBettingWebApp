using BLL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BLL.Tests
{
    [TestClass]
    public class BetModelUnitTest
    {
        [TestMethod]
        public void Bet_Should_SetValidProperties()
        {
            var bet = new Bet();
            var now = DateTime.UtcNow;

            bet.BetId = 1;
            bet.UserId = 2;
            bet.MatchId = 3;
            bet.BetType = "1";
            bet.Stake = 50;
            bet.Odds = 2.0m;
            bet.Status = "Pending";
            bet.PlacedAt = now;

            Assert.AreEqual(1, bet.BetId);
            Assert.AreEqual(2, bet.UserId);
            Assert.AreEqual(3, bet.MatchId);
            Assert.AreEqual("1", bet.BetType);
            Assert.AreEqual(50, bet.Stake);
            Assert.AreEqual(2.0m, bet.Odds);
            Assert.AreEqual("Pending", bet.Status);
            Assert.AreEqual(now, bet.PlacedAt);
        }

        [TestMethod]
        public void BetId_ShouldThrowException_WhenNegative()
        {
            var bet = new Bet();
            try
            {
                bet.BetId = -1;
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Bet ID cannot be negative");
            }
        }

        [TestMethod]
        public void UserId_ShouldThrowException_WhenZeroOrNegative()
        {
            var bet = new Bet();
            try
            {
                bet.UserId = 0;
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "User ID must be positive");
            }
        }

        [TestMethod]
        public void MatchId_ShouldThrowException_WhenZeroOrNegative()
        {
            var bet = new Bet();
            try
            {
                bet.MatchId = 0;
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Match ID must be positive");
            }
        }

        [TestMethod]
        public void BetType_ShouldThrowException_WhenInvalid()
        {
            var bet = new Bet();
            try
            {
                bet.BetType = "invalid";
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "BetType must be '1', 'X', or '2'");
            }
        }

        [TestMethod]
        public void Stake_ShouldThrowException_WhenZeroOrNegative()
        {
            var bet = new Bet();
            try
            {
                bet.Stake = 0;
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Stake must be greater than 0");
            }
        }

        [TestMethod]
        public void Odds_ShouldThrowException_WhenLessThanOne()
        {
            var bet = new Bet();
            try
            {
                bet.Odds = 0.5m;
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Odds must be 1 or higher");
            }
        }

        [TestMethod]
        public void Status_ShouldThrowException_WhenEmpty()
        {
            var bet = new Bet();
            try
            {
                bet.Status = "";
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Status cannot be empty");
            }
        }

        [TestMethod]
        public void PlacedAt_ShouldThrowException_WhenInFuture()
        {
            var bet = new Bet();
            try
            {
                bet.PlacedAt = DateTime.UtcNow.AddMinutes(10);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "PlacedAt cannot be in the future");
            }
        }

        [TestMethod]
        public void BetType_ShouldAllowValidValues()
        {
            var bet = new Bet();
            bet.BetType = "1";
            Assert.AreEqual("1", bet.BetType);

            bet.BetType = "X";
            Assert.AreEqual("X", bet.BetType);

            bet.BetType = "2";
            Assert.AreEqual("2", bet.BetType);
        }
    }
}
