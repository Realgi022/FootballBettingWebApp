using BLL.Models;
using BLL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = 
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string is null");
        }

        public async Task AddUserAsync(User user)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "INSERT INTO Users (Username, Password, Email, Age) VALUES (@Username, @Password, @Email, @Age)";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Age", user.Age);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT userId, Username, Password, Email, Age, Role FROM Users WHERE Username = @Username";
            await using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    PasswordHash = reader.GetString(2),
                    Email = reader.GetString(3),
                    Age = reader.GetInt32(4),
                    Role = (UserRole)reader.GetInt32(5) 
                };
            }

            return null;
        }

    }
}
