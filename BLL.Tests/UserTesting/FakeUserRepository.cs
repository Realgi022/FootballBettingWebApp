using BLL.Interfaces;
using BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Tests.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public Task AddUserAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            return Task.FromResult(user);
        }
    }
}
