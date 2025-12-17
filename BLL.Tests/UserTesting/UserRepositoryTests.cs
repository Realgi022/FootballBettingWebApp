using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Models;
using BLL.Tests.Repositories;
using System.Threading.Tasks;

namespace BLL.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private FakeUserRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new FakeUserRepository();
        }

        [TestMethod]
        public async Task AddUserAsync_ShouldStoreUser()
        {
            // Arrange
            var user = new User
            {
                Username = "john",
                PasswordHash = "pass",
                Email = "john@test.com",
                Age = 20
            };

            // Act
            await _repo.AddUserAsync(user);
            var foundUser = await _repo.GetUserByUsernameAsync("john");

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("john", foundUser!.Username);
        }


        [TestMethod]
        public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenNotFound()
        {
            // Act
            var result = await _repo.GetUserByUsernameAsync("missing");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetUserByUsernameAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var user1 = new User { Username = "A", PasswordHash = "1", Email = "a@test.com", Age = 20 };
            var user2 = new User { Username = "B", PasswordHash = "2", Email = "b@test.com", Age = 25 };

            await _repo.AddUserAsync(user1);
            await _repo.AddUserAsync(user2);

            // Act
            var found = await _repo.GetUserByUsernameAsync("B");

            // Assert
            Assert.IsNotNull(found);
            Assert.AreEqual("B", found!.Username);
        }
    }
}
