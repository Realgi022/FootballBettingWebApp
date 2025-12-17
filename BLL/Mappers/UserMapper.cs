using BLL.DTOs;
using BLL.Models;

namespace BLL.Mappers
{
    public static class UserMapper
    {
        public static User ToModel(UserDTO dto)
        {
            return new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                Email = dto.Email,
                Age = dto.Age,
                Role = dto.Role 
            };
        }

        public static UserDTO ToDto(User user)
        {
            return new UserDTO
            {
                Username = user.Username,
                Email = user.Email,
                Age = user.Age,
                Role = user.Role 
            };
        }
    }
}
