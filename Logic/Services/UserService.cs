using BCrypt.Net;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Mappers;
using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(UserDTO userDto)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username);
            if (existingUser != null)
                return false;

            var user = UserMapper.ToModel(userDto);

            // Hash the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<User?> ValidateLoginAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
                return null;

            bool verified = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            return verified ? user : null;
        }
    }
}
