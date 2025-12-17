using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using System;

namespace BLL.Tests.UserTesting
{
    [TestClass]
    public class UserTests
    {


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Username_ShouldThrow_WhenEmpty()
        {
            var user = new User();
            user.Username = "";
        }

        [TestMethod]
        public void Username_ShouldTrimWhitespace()
        {
            var user = new User();
            user.Username = "   John   ";
            Assert.AreEqual("John", user.Username);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Password_ShouldThrow_WhenEmpty()
        {
            var user = new User();
            user.PasswordHash = "   ";
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Email_ShouldThrow_WhenInvalidFormat()
        {
            var user = new User();
            user.Email = "invalidEmail.com";
        }

        [TestMethod]
        public void Email_ShouldTrimWhitespace()
        {
            var user = new User();
            user.Email = "   test@test.com   ";
            Assert.AreEqual("test@test.com", user.Email);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Age_ShouldThrow_WhenUnder18()
        {
            var user = new User();
            user.Age = 17;
        }

        [TestMethod]
        public void Age_ShouldAccept_18andAbove()
        {
            var user = new User();
            user.Age = 18;
            Assert.AreEqual(18, user.Age);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserId_ShouldThrow_WhenNegative()
        {
            var user = new User();
            user.UserId = -10;
        }

        [TestMethod]
        public void UserId_ShouldAccept_ZeroOrPositive()
        {
            var user = new User();
            user.UserId = 5;
            Assert.AreEqual(5, user.UserId);
        }
    }
}
