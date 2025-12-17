using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using BLL.DTOs;
using BLL.Models;
using BCrypt.Net;

namespace BLL.Tests.UserTesting
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock = null!;
        private UserService _userService = null!;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }


        [TestMethod]
        public async Task RegisterUserAsync_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var dto = new UserDTO { Username = "test", Password = "123" };
            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync("test"))
                .ReturnsAsync(new User());

            // Act
            var result = await _userService.RegisterUserAsync(dto);

            // Assert
            Assert.IsFalse(result);
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task RegisterUserAsync_ShouldReturnTrue_WhenNewUserIsCreated()
        {
            // Arrange
            var dto = new UserDTO
            {
                Username = "newUser",
                Password = "123",
                Email = "test@test.com",
                Age = 20
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync("newUser"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.RegisterUserAsync(dto);

            // Assert
            Assert.IsTrue(result);
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
        }


        [TestMethod]
        public async Task ValidateLoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var login = new LoginDTO { Username = "missing", Password = "123" };

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync("missing"))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ValidateLoginAsync(login);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ValidateLoginAsync_ShouldReturnNull_WhenPasswordIncorrect()
        {
            // Arrange
            var login = new LoginDTO { Username = "user", Password = "wrong" };
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correct");

            var user = new User
            {
                Username = "user",
                PasswordHash = hashedPassword
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync("user"))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateLoginAsync(login);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ValidateLoginAsync_ShouldReturnUser_WhenPasswordCorrect()
        {
            // Arrange
            var login = new LoginDTO { Username = "user", Password = "123" };
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123");

            var user = new User
            {
                Username = "user",
                PasswordHash = hashedPassword
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByUsernameAsync("user"))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.ValidateLoginAsync(login);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user", result!.Username);
        }
    }
}